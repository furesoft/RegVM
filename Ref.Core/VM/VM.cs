using System;
using System.Collections.Generic;

namespace Ref.Core
{
    public class VM
    {
        public Dictionary<OpCode, Instruction> Instructions { get; set; } = new Dictionary<OpCode, Instruction>();
        public RegisterCollection Register { get; set; }

        public Stack Stack { get; set; }

        public VM()
        {
            Register = new RegisterCollection(this);
            Stack = new Stack();

            FindInstruction();

            ErrorTable.Add(0x1, "The Register is protected"); //ToDo: add ErrorAttribute to Instructions
        }

        public void Run(VmWriter writer)
        {
            Run(writer.ToArray());
        }

        public void Run(byte[] raw)
        {
            var r = new VmReader(raw, this);
            Register.Subscribe(Registers.IPR, _ =>
            {
                r.SetPosition(_);
            });
            Register.Subscribe(Registers.ERR, _ =>
            {
                Console.WriteLine("An Error has occured: Error-code: 0x{0:x}: {1}", _, ErrorTable.GetExplanation(_));
            });

            while (Register[Registers.IPR] < raw.Length)
            {
                RunInstructionLine(r);
            }
        }

        public void RunInstructionLine(VmReader r)
        {
            var op = (OpCode)r.ReadWord();

            if (!Instructions[op].Invoke(r, this))
            {
                Register[Registers.ERR] = 1;
            }
        }

        private void FindInstruction()
        {
            var types = GetType().Assembly.GetTypes();
            foreach (var t in types)
            {
                if (t.BaseType == typeof(Instruction))
                {
                    var instance = (Instruction)Activator.CreateInstance(t);

                    Instructions.Add(instance.OPCode, instance);
                }
            }
        }
    }
}