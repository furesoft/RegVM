using System;
using System.IO;

namespace Ref.Core
{
    public class VmWriter
    {
        public VmWriter()
        {
            ms = new MemoryStream();
            bw = new BinaryWriter(ms);
        }

        public byte[] ToArray()
        {
            return ms.ToArray();
        }

        public void Write(byte value)
        {
            bw.Write(value);
        }

        public void Write(short value)
        {
            bw.Write(BitConverter.GetBytes(value));
        }

        public void Write(int value)
        {
            var operand = new Operand();
            operand.Type = OperandType.Value;
            operand.Value = value;

            Write(operand);
        }

        public void Write(Operand value)
        {
            bw.Write((byte)value.Type);

            switch (value.Type)
            {
                case OperandType.Value:
                    bw.Write(BitConverter.GetBytes((int)value.Value));
                    break;

                case OperandType.Addr:
                    Write((short)value.Value);
                    break;

                case OperandType.Label:
                    break;

                case OperandType.Register:
                    Write((byte)value.Value);
                    break;
            }
        }

        public void Write(OpCode op, Registers a, Registers b, Registers c)
        {
            Write(op);

            Write(a);
            Write(b);
            Write(c);
        }

        public void Write(OpCode op, Registers reg, int v)
        {
            Write(op);

            Write(reg, v);
        }

        public void Write(Registers value)
        {
            bw.Write((byte)value);
        }

        public void Write(OpCode value)
        {
            bw.Write((byte)value);
        }

        public void Write(Registers reg, int value)
        {
            var opReg = new Operand();
            opReg.Type = OperandType.Register;
            opReg.Value = reg;

            Write(opReg);

            var opVal = new Operand();
            opVal.Type = OperandType.Value;
            opVal.Value = value;

            Write(opVal);
        }

        public void Write(OpCode op, Registers a)
        {
            Write((byte)op);
        }

        private BinaryWriter bw;
        private MemoryStream ms;
    }
}