using Ref.Core;
using System;
using System.Linq;

namespace Ref_Repl
{
    public class Repl
    {
        public Repl()
        {
            vm = new VM();
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

                if (ReplCommand.IsCommand(input))
                {
                    var cmd = ReplCommand.Parse(input);
                    switch (cmd.Name)
                    {
                        case "mode":
                            var arg = cmd.Args.First();
                            if (arg == "asm")
                            {
                                ishexMode = false;
                            }
                            else if (arg == "hex")
                            {
                                ishexMode = true;
                            }
                            else
                            {
                                Console.WriteLine($"mode '{arg}' not recognized");
                            }

                            break;

                        case "register":
                            Utils.PrintRegisters(vm.Register);

                            break;

                        case "clear":
                            Console.Clear();
                            break;

                        case "explain":
                            if (cmd.Args.Length == 1)
                            {
                                var errorcodeStr = cmd.Args.First();

                                Console.WriteLine(ErrorTable.GetExplanation(int.Parse(errorcodeStr)));
                            }
                            else
                            {
                                Console.WriteLine("Please specifiy an errorcode");
                            }
                            break;

                        default:
                            Console.WriteLine($"Command '{cmd.Name}' not found");
                            break;
                    }
                }
                else
                {
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

                        vm.Register[Registers.IPR] = 0;
                    }
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