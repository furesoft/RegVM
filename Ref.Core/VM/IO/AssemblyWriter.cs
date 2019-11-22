using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ref.Core.VM.IO
{
    public class AssemblyWriter
    {
        public List<AssemblySection> Sections { get; set; }

        public byte[] Save()
        {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);

            bw.Write(0xA33);

            bw.Write(Sections.Count);

            foreach (var s in Sections)
            {
                bw.Write(s.Name);
                bw.Write(s.Raw.Length);
                bw.Write(s.Raw);
            }

            bw.Close();

            return ms.ToArray();
        }
    }
}