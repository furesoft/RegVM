using LibObjectFile.Elf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ref.Core.VM.IO
{
    public class AssemblyWriter
    {
        public ElfObjectFile Elf = new ElfObjectFile();

        public AssemblyWriter()
        {
            Elf.AddSection(new ElfSectionHeaderStringTable());
        }

        public void CreateCodeSection(CommandWriter cmdBuffer)
        {
            var code = cmdBuffer.Save();
            var codeSection = new ElfCustomSection(new MemoryStream(code)).ConfigureAs(ElfSectionSpecialType.Text);
            Elf.AddSection(codeSection);
        }

        public void CreateMetaSection(AssemblyInfo info)
        {
            var metaSection = new ElfCustomSection(new MemoryStream(info.Serialize())).ConfigureAs(ElfSectionSpecialType.Data);

            Elf.AddSection(metaSection);
        }

        public byte[] Save()
        {
            var ms = new MemoryStream();

            Elf.Write(ms);

            return ms.ToArray();
        }

        public Stream SaveToStream()
        {
            var ms = new MemoryStream();

            Elf.Write(ms);

            return ms;
        }
    }
}