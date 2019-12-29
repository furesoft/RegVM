using LibObjectFile.Elf;
using Ref.Core.Parser;
using Ref.Core.VM;
using Ref.Core.VM.Core;
using Ref.Core.VM.Core.Interrupts;
using Ref.Core.VM.Core.MappedIO;
using Ref.Core.VM.Core.Ports;
using Ref.Core.VM.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Ref.Core
{
    public unsafe class VirtualMachine
    {
        public ElfObjectFile Assembly { get; set; }
        public Debugger Debugger { get; set; } = new Debugger();

        public Dictionary<OpCode, Instruction> Instructions { get; set; } = new Dictionary<OpCode, Instruction>();
        public RegisterCollection Register { get; set; }
        public Stack Stack { get; set; }

        public VirtualMachine(ElfObjectFile file)
        {
            Assembly = file;
            Register = new RegisterCollection(this);
            Stack = new Stack();

            ScanInstructions();
            PortMappedDeviceManager.ScanDevices(this);
            InterruptTable.ScanHandlers();
            MemoryMappedDeviceManager.ScanDevices();
            ErrorTable.ScanErrors();
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

            if (!Debugger.HasBreakPoint(Register[Registers.IPR]))
            {
                if (!RunInstruction(cmd))
                {
                    Register[Registers.ERR] = 1;
                }
            }
        }

        public void Run()
        {
            var start = Assembly.EntryPointAddress;
            var code = Assembly.GetSection<ElfCustomSection>(ElfSectionSpecialType.Text);
            Run(code.Stream, start);
        }

        public void Run(Stream strm, ulong startAddress = 0)
        {
            var r = new BinaryReader(strm);
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
                Register.SetValue(Registers.IPR, (int)startAddress);
            }

            while (Register[Registers.IPR] < strm.Length)
            {
                ParseInstruction(r);
            }
        }

        public void SetMemoryOf<T>(int start, int value)
                            where T : IMemory
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
                    break;

                default:
                    result = null;
                    break;
            }

            result.SetValue(start, value);
        }

        public byte[] ViewMemoryOf<T>(int n = 10)
            where T : IMemory
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