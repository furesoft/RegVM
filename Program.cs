using System;

namespace RefVM
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sr = "MOV $A, #2A";
            var res = AssemblyLine.Parse(sr);

            var vm = new VM();
            var program = new byte[] {
                (byte)OpCode.MOV, (byte)OperandType.Register,(byte)Registers.A, (byte)OperandType.Value,42,0,0,0,
                (byte)OpCode.MOV, (byte)OperandType.Register,(byte)Registers.B, (byte)OperandType.Value,4,0,0,0,
                (byte)OpCode.DIV, (byte)OperandType.Register,(byte)Registers.A,(byte)OperandType.Register,(byte)Registers.B,(byte)OperandType.Register,(byte)Registers.C,
            };

            //ToDo: fix writer
            var p = new VmWriter();
            p.Write(OpCode.MOV, Registers.A, 42);
            p.Write(OpCode.MOV, Registers.B, 4);
            p.Write(OpCode.DIV, Registers.A, Registers.B, Registers.C);

            var repl = new Repl();
            repl.Run();

            Console.ReadLine();
        }
    }
}