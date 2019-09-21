using System.Collections.Generic;

namespace Ref.Core
{
    public class AssemblySource
    {
        public List<AssemblyLine> Lines { get; set; } = new List<AssemblyLine>();

        public static AssemblySource Parse(string src)
        {
            var res = new AssemblySource();
            var spl = src.Split(new char[] { ';', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in spl)
            {
                res.Lines.Add(AssemblyLine.Parse(line));
            }

            return res;
        }

        public byte[] Assemble()
        {
            var res = new List<byte>();
            foreach (var line in Lines)
            {
                res.AddRange(line.Assemble());
            }

            return res.ToArray();
        }
    }
}