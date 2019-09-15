using System;
using System.IO;

namespace RefVM
{
    public class VmReader
    {
        public VM Vm { get; }

        public VmReader(byte[] raw, VM vm)
        {
            reader = new BinaryReader(new MemoryStream(raw));
            Vm = vm;
        }

        public short ReadDWord()
        {
            Vm.Register[(int)Registers.IPR].Increment();
            Vm.Register[(int)Registers.IPR].Increment();

            return reader.ReadInt16();
        }

        public Operand ReadOperand()
        {
            var res = new Operand();
            var optype = (OperandType)ReadWord();

            res.Type = optype;

            switch (optype)
            {
                case OperandType.None:
                    break;

                case OperandType.Value:
                    res.Value = ReadQWord();
                    break;

                case OperandType.Addr:
                    res.Value = ReadDWord();
                    break;

                case OperandType.Label:
                    break;

                case OperandType.Register:
                    res.Value = ReadWord();
                    break;
            }

            return res;
        }

        public int ReadQWord()
        {
            Vm.Register[(int)Registers.IPR].Increment();
            Vm.Register[(int)Registers.IPR].Increment();
            Vm.Register[(int)Registers.IPR].Increment();
            Vm.Register[(int)Registers.IPR].Increment();

            return reader.ReadInt32();
        }

        public byte ReadWord()
        {
            Vm.Register[(int)Registers.IPR].Increment();
            return reader.ReadByte();
        }

        public void SetPosition(int addr)
        {
            reader.BaseStream.Position = addr;
        }

        private BinaryReader reader;
    }
}