using System;
using System.IO;

namespace Ref.Core.VM.IO
{
    public class AssemblyInfo
    {
        public Guid ID { get; set; }
        public string Version { get; set; }

        public static AssemblyInfo Deserialize(byte[] raw)
        {
            var br = new BinaryReader(new MemoryStream(raw));
            var res = new AssemblyInfo();
            res.ID = new Guid(br.ReadBytes(16));
            res.Version = br.ReadString();

            return res;
        }

        public byte[] Serialize()
        {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);

            bw.Write(ID.ToByteArray());
            bw.Write(Version);

            return ms.ToArray();
        }
    }
}