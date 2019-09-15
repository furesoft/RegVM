using RefVM.Parser;
using System;

namespace RefVM
{
    public class Repl
    {
        public Repl()
        {
            vm = new VM();
        }

        public static void PrintRegisters(Register[] register)
        {
            for (int i = 0; i < register.Length; i++)
            {
                var reg = Enum.GetName(typeof(Registers), i);
                var val = register[i];

                Console.WriteLine("{0,10}{1,10:x4}", reg, val.GetValue());
            }
        }

        public void Run()
        {
            ReadLine.HistoryEnabled = true;
            ReadLine.AutoCompletionHandler = new AutoCompletionHandler();

            bool ishexMode = false;

            while (true)
            {
                string prefix = ishexMode ? "hex" : "asm";

                var input = ReadLine.Read(prefix + "> ");
                if (input == ".register")
                {
                    PrintRegisters(vm.Register);

                    continue;
                }
                if (input == ".mode asm")
                {
                    ishexMode = false;
                    continue;
                }
                if (input == ".mode hex")
                {
                    ishexMode = true;
                    continue;
                }
                if (input == ".clear")
                {
                    Console.Clear();
                }

                if (ishexMode)
                {
                    var prog = ParseHex(input);
                    var reader = new VmReader(prog, vm);
                    vm.RunInstructionLine(reader);
                }
                else
                {
                    var src = AssemblySource.Parse(input);
                    var writer = new VmWriter();

                    foreach (var line in src.Lines)
                    {
                        writer.Write(line.Opcode);

                        foreach (var operand in line.Operands)
                        {
                            writer.Write(operand);
                        }
                    }

                    vm.Run(writer);
                    vm.SetValue(Registers.IPR, 0);
                }
            }
        }

        private VM vm;

        private byte[] ParseHex(string src)
        {
            var chunks = src.Split(' ');
            var res = new byte[chunks.Length];

            for (int i = 0; i < chunks.Length - 1; i++)
            {
                res[i] = Convert.ToByte(chunks[i], 16);
            }

            return res;
        }
    }
}