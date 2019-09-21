using System.Collections.Generic;

namespace Ref.Core
{
    public class Heap
    {
        public LinkedList<HeapSegment> FreeList { get; set; }

        public Heap()
        {
            FreeList = new LinkedList<HeapSegment>();
        }

        public int Allocate(int size)
        {
        }

        public void Free(int address)
        {
        }
    }
}