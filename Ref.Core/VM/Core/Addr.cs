namespace Ref.Core.VM.Core
{
    public struct Addr
    {
        public Addr(int addr, Heap h)
        {
            this._value = addr;
            this._heap = h;
        }

        public object Dereference()
        {
            _heap.storage.Find(_value).;
        }

        private Heap _heap { get; set; }
        private int _value { get; set; }
    }
}