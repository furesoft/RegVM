using System;
using System.Collections.Generic;
using System.IO;

namespace Ref.Core.VM.IO
{
    public class Assembly
    {
        public List<AssemblySection> Sections { get; set; } = new List<AssemblySection>();

        public AssemblySection this[AssemblySections section]
        {
            get
            {
                foreach (var s in Sections)
                {
                    if (s.Name == Enum.GetName(typeof(AssemblySections), section))
                    {
                        return s;
                    }
                }

                return Sections[0];
            }
        }

        public static Assembly Load(byte[] raw)
        {
            var r = new BinaryReader(new MemoryStream(raw));
            var result = new Assembly();

            var magic = r.ReadInt32();
            if (magic == 0xA33)
            {
                // Load Sections
                var count = r.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    var sect = new AssemblySection();
                    sect.Name = r.ReadString();

                    var rawCount = r.ReadInt32();
                    sect.Raw = r.ReadBytes(rawCount);

                    result.Sections.Add(sect);
                }
            }
            else
            {
                throw new Exception("Unknown Assembly Format");
            }

            r.Close();

            return result;
        }
    }

    public class AssemblySection
    {
        public string Name { get; set; }
        public byte[] Raw { get; set; } = new byte[0];

        public override string ToString()
        {
            return Name;
        }
    }
}