using System;
using System.Collections.Generic;
using System.Linq;

namespace Ref.Core
{
    public class AssemblyLine
    {
        public OpCode Opcode { get; set; }
        public List<Operand> Operands { get; set; } = new List<Operand>();

        public static AssemblyLine Parse(string src)
        {
            var line = new AssemblyLine();
            line.Opcode = (OpCode)Enum.Parse(typeof(OpCode), src.Substring(0, 3).ToUpper());

            if (src.Length > 3)
            {
                var argList = src.Substring(3);
                var argSpl = argList.Split(',').Select(_ => _.Trim());

                foreach (var arg in argSpl)
                {
                    var type = arg.Substring(0, 1);
                    var val = arg.Substring(1);

                    var op = new Operand();
                    if (type == "$")
                    {
                        op.Type = OperandType.Register;
                        op.Value = Enum.Parse(typeof(Registers), val.ToUpper());
                    }
                    else if (type == "#")
                    {
                        op.Type = OperandType.Value;

                        if (val.StartsWith("0x"))
                        {
                            op.Value = Convert.ToInt32(val.Substring(2), 16);
                        }
                        else
                        {
                            op.Value = Convert.ToInt32(val, 16);
                        }
                    }

                    line.Operands.Add(op);
                }
            }

            return line;
        }

        public byte[] Assemble()
        {
            var w = new VmWriter();

            w.Write(Opcode);

            foreach (var operand in Operands)
            {
                w.Write(operand);
            }

            return w.ToArray();
        }
    }
}