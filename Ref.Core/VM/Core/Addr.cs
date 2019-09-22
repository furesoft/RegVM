using System;

namespace Ref.Core
{
    public struct Addr
    {
        public int Length { get; }

        public Addr(uint addr, Heap h, int length)
        {
            this._value = addr;
            this._heap = h;
            this.Length = length;
        }

        public object Dereference()
        {
            byte[] dest = new byte[Length];
            _heap.storage.Find(_value).Read(dest, 0, 0, Length);

            return dest; //ToDo: convert byte[] to real object
        }

        public uint ToUInt()
        {
            return _value;
        }

        private Heap _heap { get; set; }
        private uint _value { get; set; }
    }
}