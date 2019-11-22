using System.Linq;

namespace Ref.Core.VM.IO
{
    public class Memory
    {
        public virtual byte[] GetMemory()
        {
            return null;
        }

        public byte[] Slice(int n)
        {
            return GetMemory().Take(n).ToArray();
        }
    }
}