using System;
using System.Collections.Generic;

namespace RefVM
{
    public class VM
    {
        public Dictionary<OpCode, Instruction> Instructions { get; set; } = new Dictionary<OpCode, Instruction>();
        public Register[] Register { get; set; }

        public VM()
        {
            Register = new Register[Enum.GetNames(typeof(Registers)).Length];
            InitRegisters();

            Instructions.Add(OpCode.MOV, new MovInstruction());

            Instructions.Add(OpCode.ADD, new AddInstruction());
            Instructions.Add(OpCode.SUB, new SubInstruction());

            Instructions.Add(OpCode.DIV, new DivInstruction());
            Instructions.Add(OpCode.MUL, new MulInstruction());

            Instructions.Add(OpCode.JMP, new JumpInstruction());
            Instructions.Add(OpCode.PNT, new PrintRegisterInstruction());

            Instructions.Add(OpCode.CMP, new CompareInstruction());

            ErrorTable.Add(0x1, "The Register is protected");
        }

        public void ClearRegister(Registers value)
        {
            SetValue(value, 0);
        }

        public int GetValue(Registers reg)
        {
            return Register[BitConverter.ToInt32(new byte[] { (byte)reg, 0, 0, 0 }, 0)].GetValue();
        }

        public void Run(VmWriter writer)
        {
            Run(writer.ToArray());
        }

        public void Run(byte[] raw)
        {
            var r = new VmReader(raw, this);
            Subscribe(Registers.IPR, _ =>
            {
                r.SetPosition(_);
            });
            Subscribe(Registers.ERR, _ =>
            {
                Console.WriteLine("An Error has occured: Error-code: 0x{0:x}: {1}", _, ErrorTable.GetExplanation(_));
            });

            while (Register[(int)Registers.IPR].GetValue() < raw.Length)
            {
                RunInstructionLine(r);
            }
        }

        public void RunInstructionLine(VmReader r)
        {
            var op = (OpCode)r.ReadWord();

            if (!Instructions[op].Invoke(r, this))
            {
                SetValue(Registers.ERR, 1);
            }
        }

        public void SetValue(Registers value, int v)
        {
            var index = BitConverter.ToInt32(new byte[] { (byte)value, 0, 0, 0 }, 0);
            Register[index].SetValue(v);
        }

        public void Subscribe(Registers reg, Action<int> callback)
        {
            Register[(int)reg].OnChange = callback;
        }

        private void InitRegisters()
        {
            for (int i = 0; i < Register.Length; i++)
            {
                var reg = new RefVM.Register();
                reg.Vm = this;
                reg.Access = RefVM.Register.RegisterAccess.Read | RefVM.Register.RegisterAccess.Write;

                Register[i] = reg;
            }

            Register[(int)Registers.F].Access = Register[(int)Registers.F].Access | RefVM.Register.RegisterAccess.Protected;
        }
    }
}