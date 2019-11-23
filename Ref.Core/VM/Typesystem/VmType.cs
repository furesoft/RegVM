using System.Collections.Generic;
using System.IO;

namespace Ref.Core.VM.Typesystem
{
    public class VmType : ITypesystemSerializer
    {
        public VmType BaseType { get; set; }
        public List<VmField> Fields { get; set; } = new List<VmField>();
        public string Name { get; set; }

        public void Deserialize(byte[] raw)
        {
            var br = new BinaryReader(new MemoryStream(raw));
            Name = br.ReadString();
        }

        public byte[] Serizalize()
        {
            throw new System.NotImplementedException();
        }
    }
}