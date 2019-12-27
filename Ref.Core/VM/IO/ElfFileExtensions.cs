using LibObjectFile.Elf;
using System.Linq;

namespace Ref.Core.VM.IO
{
    public static class ElfFileExtensions
    {
        public static T GetSection<T>(this ElfObjectFile file, string name)
            where T : ElfSection
        {
            foreach (var sec in file.Sections)
            {
                if (sec.Name.Value == name)
                {
                    return (T)sec;
                }
            }
            return (T)file.Sections.Where(_ => _.Name.Value == name).FirstOrDefault();
        }

        public static T GetSection<T>(this ElfObjectFile file, ElfSectionSpecialType sectionSpecialType)
            where T : ElfSection
        {
            string name = "";
            switch (sectionSpecialType)
            {
                case ElfSectionSpecialType.Bss:
                    name = ".bss";

                    break;

                case ElfSectionSpecialType.Comment:
                    name = ".comment";
                    break;

                case ElfSectionSpecialType.Data:
                    name = ".data";
                    break;

                case ElfSectionSpecialType.Data1:
                    name = ".data1";
                    break;

                case ElfSectionSpecialType.Debug:
                    name = ".debug";
                    break;

                case ElfSectionSpecialType.Dynamic:
                    name = ".dynamic";
                    break;

                case ElfSectionSpecialType.DynamicStringTable:
                    name = ".dynstr";
                    break;

                case ElfSectionSpecialType.DynamicSymbolTable:
                    name = ".dynsym";
                    break;

                case ElfSectionSpecialType.Init:
                    name = ".init";
                    break;

                case ElfSectionSpecialType.Interp:
                    name = ".interp";
                    break;

                case ElfSectionSpecialType.Line:
                    name = ".line";
                    break;

                case ElfSectionSpecialType.Relocation:
                    name = ElfRelocationTable.DefaultName;
                    break;

                case ElfSectionSpecialType.RelocationAddends:
                    name = ElfRelocationTable.DefaultNameWithAddends;
                    break;

                case ElfSectionSpecialType.ReadOnlyData:
                    name = ".rodata";
                    break;

                case ElfSectionSpecialType.ReadOnlyData1:
                    name = ".rodata1";
                    break;

                case ElfSectionSpecialType.SectionHeaderStringTable:
                    name = ".shstrtab";
                    break;

                case ElfSectionSpecialType.StringTable:
                    name = ElfStringTable.DefaultName;
                    break;

                case ElfSectionSpecialType.SymbolTable:
                    name = ElfSymbolTable.DefaultName;
                    break;

                case ElfSectionSpecialType.Text:
                    name = ".text";
                    break;
            }

            return (T)file.Sections.Where(_ => _.Name.Value == name).FirstOrDefault();
        }
    }
}