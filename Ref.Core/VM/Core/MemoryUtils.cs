namespace Ref.Core.VM.Core
{
    public static unsafe class MemoryUtils
    {
        public static void MemSet(byte* buffer, byte value, int size)
        {
            for (int i = 0; i < size; i++)
            {
                *(buffer + i) = value;
            }
        }

        public static void* ZeroMem(void* ptr, int size)
        {
            MemSet((byte*)ptr, 0, size);
            return ptr;
        }
    }
}