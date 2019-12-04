using System;

namespace Ref.Core.VM.IO.MappedIO
{
    public class AddressRangeAttribute : Attribute
    {
        public int End { get; set; }

        public int Start { get; set; }

        public AddressRangeAttribute(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}