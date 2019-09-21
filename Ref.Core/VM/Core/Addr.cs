using System;

namespace Ref.Core
{
    public struct Addr
    {
        public Addr(uint addr, Heap h)
        {
            this._value = addr;
            this._heap = h;
        }

        public object Dereference()
        {
            _heap.storage.Find(_value);
        }

        public uint ToUInt()
        {
            return _value;
        }

        private Heap _heap { get; set; }
        private uint _value { get; set; }
    }
}