using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Ref.Core.VM.Core
{
    public static unsafe class Heap
    {
        public static void* Alloc(int size)
        {
            return Marshal.AllocHGlobal(size).ToPointer();
        }

        public static void* AllocZeroed(int size)
        {
            void* result = Alloc(size);
            if (result == null)
            {
                return null;
            }
            return MemoryUtils.ZeroMem(result, size);
        }

        public static void Free(void* ptr)
        {
            Marshal.FreeHGlobal(new IntPtr(ptr));
        }
    }
}