namespace Ref.Core.VM.IO
{
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