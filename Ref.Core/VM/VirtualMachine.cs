﻿using Ref.Core.Parser;
using Ref.Core.VM.IO;
using System;
using System.IO;

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
    }

    public class VirtualMachine
    {
        public RegisterCollection Register { get; set; }

        public Stack Stack { get; set; }

        public VirtualMachine()
        {
            Register = new RegisterCollection(this);
            Stack = new Stack();

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

        public void Run(byte[] raw)
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
                case OpCode.MOV:
                    var reg = cmd[0];
                    var val = cmd[1];

                    Register[(Registers)reg] = val;

                    break;

                case OpCode.JMP:
                    var addr = cmd[0];
                    Register[Registers.IPR] = addr;

                    break;

                case OpCode.JMPR:
                    var jmp_index = cmd[0];

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
                    var add_left = Register[(Registers)cmd[0]];
                    var add_right = Register[(Registers)cmd[1]];

                    Register[Registers.ACC] = add_left + add_right;

                    break;

                case OpCode.PRINT:
                    Utils.PrintRegisters(Register);
                    break;

                case OpCode.PUSH:
                    var from = cmd[0];
                    Stack.Push(Register[(Registers)from]);

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
    }
}