using Ref.Core.Parser;
using Ref.Core.VM;
using Ref.Core.VM.Core;
using Ref.Core.VM.Core.Interrupts;
using Ref.Core.VM.Core.Ports;
using Ref.Core.VM.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Ref.Core
{
    public enum OpCode
    {
        MOV,
        ADD,
        DIV,
        MUL,
        SUB,
        NOP,
        JMPR,
        PRINT,
        CMP,
        PUSH,
        POP,
        JMP,
        LOAD,
        CALL,
        RET,
        OUT,
        IN,
        PUSHL,
        JMPE,
        JMPNE,
        EQUAL,
        NEQUAL,
        INT,
    }

    public class VirtualMachine
    {
        public Dictionary<OpCode, Instruction> Instructions { get; set; } = new Dictionary<OpCode, Instruction>();
        public RegisterCollection Register { get; set; }

        public Stack Stack { get; set; }

        public VirtualMachine()
        {
            Register = new RegisterCollection(this);
            Stack = new Stack();

            ScanInstructions();
            PortMappedDeviceManager.ScanDevices();
            InterruptTable.ScanHandlers();

            ErrorTable.Add(0x1, "The Register is protected"); //ToDo: add ErrorAttribute to Instructions
        }

        public void ParseInstruction(BinaryReader r)
        {
            var cmd = new AsmCommand();
            cmd.OpCode = (OpCode)r.ReadInt32();

            var argCount = r.ReadInt32();

            Register[Registers.IPR] += sizeof(int) * 2;

            for (int i = 0; i < argCount; i++)
            {
                var arg = new AsmCommandArg();
                arg.Value = r.ReadInt32();
                Register[Registers.IPR] += sizeof(int);

                cmd.Args.Add(arg);
            }

            if (!RunInstruction(cmd))
            {
                Register[Registers.ERR] = 1;
            }
        }

        public void Run(byte[] raw, int startAddress = 0)
        {
            //ToDo: implement custom file format

            var r = new BinaryReader(new MemoryStream(raw));
            Register.Subscribe(Registers.IPR, _ =>
            {
                r.BaseStream.Position = _;
            });
            Register.Subscribe(Registers.ERR, _ =>
            {
                Console.WriteLine("An Error has occured: Error-code: 0x{0:x}: {1}", _, ErrorTable.GetExplanation(_));
            });

            if (startAddress != 0)
            {
                Register.SetValue(Registers.IPR, startAddress);
            }

            while (Register[Registers.IPR] < raw.Length)
            {
                ParseInstruction(r);
            }
        }

        public byte[] ViewMemoryOf<T>(int n = 10)
        {
            var memoryType = typeof(T).Name;
            Memory result;

            switch (memoryType)
            {
                case nameof(Stack):
                    result = Stack.Memory;
                    break;

                case nameof(Register):
                    result = Register.Memory;
                    n = Register.Length * sizeof(int);
                    break;

                default:
                    result = null;
                    break;
            }

            return result.Slice(n);
        }

        private bool RunInstruction(AsmCommand cmd)
        {
            if (Instructions.ContainsKey(cmd.OpCode))
            {
                Instructions[cmd.OpCode].Invoke(cmd, this);
            }
            else
            {
                throw new Exception($"Instructon '{Enum.GetName(typeof(OpCode), cmd.OpCode)}' not found");
            }

            return true;
        }

        private void ScanInstructions()
        {
            foreach (var t in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t?.BaseType?.Name == nameof(Instruction))
                {
                    var instance = (Instruction)Activator.CreateInstance(t);

                    Instructions.Add(instance.OpCode, instance);
                }
            }
        }
    }
}