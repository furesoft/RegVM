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
            switch (cmd.OpCode)
            {
                case OpCode.LOAD:
                    var reg = (int)cmd[0];
                    var val = cmd[1];

                    Register[(Registers)reg] = (int)val;

                    break;

                case OpCode.INT:
                    var interrupt = (int)cmd[0];
                    InterruptTable.Interrupt(interrupt, this);

                    break;

                case OpCode.OUT:
                    var out_addr = (int)cmd[0];
                    var out_value = (int)cmd[1];

                    PortMappedDeviceManager.Write(out_addr, out_value, this);

                    break;

                case OpCode.IN:
                    var in_addr = (int)cmd[0];
                    var in_reg = (Registers)(int)cmd[1];

                    PortMappedDeviceManager.Read(in_addr, in_reg, this);

                    break;

                case OpCode.MOV:
                    var fromReg = (Registers)(int)cmd[0];
                    var toReg = (Registers)(int)cmd[1];

                    Register[toReg] = Register[fromReg];

                    break;

                case OpCode.JMP:
                    var addr = (int)cmd[0];
                    Register[Registers.IPR] = addr;

                    break;

                case OpCode.JMPE:
                    var e_addr = (int)cmd[0];
                    if (Register[Registers.BRR] != 0)
                    {
                        Register[Registers.IPR] = e_addr;
                    }
                    break;

                case OpCode.EQUAL:
                    var eq_f = (Registers)(int)cmd[0];
                    var eq_s = (Registers)(int)cmd[1];

                    Register[Registers.BRR] = eq_f == eq_s ? 1 : 0;

                    break;

                case OpCode.NEQUAL:
                    var neq_f = (Registers)(int)cmd[0];
                    var neq_s = (Registers)(int)cmd[1];

                    Register[Registers.BRR] = neq_f != neq_s ? 1 : 0;

                    break;

                case OpCode.JMPNE:
                    var ne_addr = (int)cmd[0];
                    if (Register[Registers.BRR] == 0)
                    {
                        Register[Registers.IPR] = ne_addr;
                    }

                    break;

                case OpCode.JMPR:
                    var jmp_index = (int)cmd[0];

                    if (jmp_index < 0)
                    {
                        Register[Registers.IPR] -= jmp_index;
                    }
                    else
                    {
                        Register[Registers.IPR] += jmp_index;
                    }

                    break;

                case OpCode.ADD:
                    var add_left = Register[(Registers)(int)cmd[0]];
                    var add_right = Register[(Registers)(int)cmd[1]];

                    Register[Registers.ACC] = add_left + add_right;

                    break;

                case OpCode.CALL:
                    var cll = (int)cmd[0];
                    Stack.PushRegisters(Register);
                    Register[Registers.IPR] = cll;

                    break;

                case OpCode.RET:
                    Stack.PopRegisters(Register);
                    break;

                case OpCode.PRINT:
                    Thread.Sleep(1500); //for demo propose only, should be removed
                    Console.Clear();

                    Utils.PrintRegisters(Register);
                    break;

                case OpCode.PUSH:
                    var from = (int)cmd[0];
                    Stack.Push(Register[(Registers)from]);

                    break;

                case OpCode.PUSHL:
                    var push_lit = (int)cmd[0];
                    Stack.Push(push_lit);

                    break;

                case OpCode.POP:
                    var to = cmd[0];
                    var rval = Stack.Pop();

                    Register[(Registers)to] = rval;
                    break;

                case OpCode.NOP:
                    break;

                default:
                    return false;
            }

            return true;
        }

        private void ScanInstructions()
        {
            foreach (var t in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.BaseType.Name == nameof(Instruction))
                {
                    var instance = (Instruction)Activator.CreateInstance(t);

                    Instructions.Add(instance.OpCode, instance);
                }
            }
        }
    }
}