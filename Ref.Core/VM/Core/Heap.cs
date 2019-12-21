using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Ref.Core.VM.Core
{
    //This implementation is based off of Leonard Kevin McGuire Jr's Binary Heap Implementation
    //found at http://wiki.osdev.org/User:Pancakes/BitmapHeapImplementation
    //(www.kmcg3413.net) (kmcg3413@gmail.com)

    /// <summary>
    ///     Represents a block of memory that has been allocated for use by the heap.
    /// </summary>
    public unsafe struct HeapBlock
    {
        /// <summary>
        ///     The size of the chunks to use when allocating memory.
        /// </summary>
        public int bsize;

        public bool expanding;

        /// <summary>
        ///     Used for optimisation.
        /// </summary>
        public int lfb;

        /// <summary>
        ///     A pointer to the next heap block.
        /// </summary>
        public HeapBlock* next;

        /// <summary>
        ///     The size of the block of memory allocated.
        /// </summary>
        public int size;

        /// <summary>
        ///     The amount of memory in the block that has been used.
        /// </summary>
        public int used;
    }

    /// <summary>
    ///     The kernel heap - currently a very simple implementation.
    /// </summary>
    public static unsafe class Heap
    {
        public static SpinLock? AccessLock;
        public static bool AccessLockInitialised;
        public static String name = "[UNINITIALISED]";
        public static bool OutputTrace;
        public static bool PreventAllocation = false;
        public static String PreventReason = "[NONE]";

        /// <summary>
        ///     A pointer to the most-recently added heap block.
        /// </summary>
        public static HeapBlock* FBlock
        {
            get { return fblock; }
        }

        static Heap()
        {
        }

        public static int AddBlock(HeapBlock* b)
        {
            bool ShouldExitCritical = false;
            if (fblock != null)
            {
                if (!fblock->expanding)
                {
                    EnterCritical("AddBlock");
                    ShouldExitCritical = true;
                }
            }

            b->next = fblock;
            fblock = b;

            if (ShouldExitCritical)
            {
                ExitCritical();
            }

            return 1;
        }

        public static void* Alloc(int size, String caller)
        {
            return Alloc(size, 1, caller);
        }

        public static void* Alloc(int size, int boundary, String caller)
        {
            EnterCritical("Alloc");

            int retry = 1;

            do
            {
                HeapBlock* b = null;
                byte* bm = null;
                int bcnt = 0;
                int x, y, z = 0;
                int bneed = 0;
                byte nid = 0;

                if (boundary > 1)
                {
                    size += boundary - 1;
                }

                /* iterate blocks */
                for (b = fblock; (int)b != 0; b = b->next)
                {
                    /* check if block has enough room */
                    if (b->size - b->used * b->bsize >= size)
                    {
                        bcnt = b->size / b->bsize;
                        bneed = size / b->bsize * b->bsize < size ? size / b->bsize + 1 : size / b->bsize;
                        bm = (byte*)&b[1];

                        for (x = b->lfb + 1 >= bcnt ? 0 : b->lfb + 1; x != b->lfb; ++x)
                        {
                            /* just wrap around */
                            if (x >= bcnt)
                            {
                                x = 0;
                            }

                            if (bm[x] == 0)
                            {
                                /* count free blocks */
                                for (y = 0; bm[x + y] == 0 && y < bneed && x + y < bcnt; ++y) ;

                                /* we have enough, now allocate them */
                                if (y == bneed)
                                {
                                    /* find ID that does not match left or right */
                                    nid = GetNID(bm[x - 1], bm[x + y]);

                                    /* allocate by setting id */
                                    for (z = 0; z < y; ++z)
                                    {
                                        bm[x + z] = nid;
                                    }

                                    /* optimization */
                                    b->lfb = x + bneed - 2;

                                    /* count used blocks NOT bytes */
                                    b->used += y;

                                    void* result = (void*)(x * b->bsize + (int)&b[1]);
                                    if (boundary > 1)
                                    {
                                        result = (void*)(((int)result + (boundary - 1)) & ~(boundary - 1));

                                        //#if HEAP_TRACE
                                        //                                      ExitCritical();
                                        //                                      BasicConsole.WriteLine(((Framework.String)"Allocated address ") + (int)result + " on boundary " + boundary + " for " + caller);
                                        //                                      EnterCritical("Alloc:Boundary condition");
                                        //#endif
                                    }

                                    ExitCritical();
                                    return result;
                                }

                                /* x will be incremented by one ONCE more in our FOR loop */
                                x += y - 1;
                            }
                        }
                    }
                }

                if (!ExpandHeap(false))
                {
                    retry--;
                }
            }
            while (retry > 0);

            {
                ExitCritical();

                return null;
            }
        }

        public static void* AllocZeroed(int size, String caller)
        {
            return AllocZeroed(size, 1, caller);
        }

        public static void* AllocZeroed(int size, int boundary, string caller)
        {
            void* result = Alloc(size, boundary, caller);
            if (result == null)
            {
                return null;
            }
            return MemoryUtils.ZeroMem(result, size);
        }

        public static void* AllocZeroedAPB(int size, int boundary, String caller)
        {
            void* result = null;
            void* oldValue = null;
            int resultAddr;
            do
            {
                oldValue = result;
                result = AllocZeroed(size, boundary, caller);
                resultAddr = (int)result;
                if (oldValue != null)
                {
                    Free(oldValue);
                }
            } while (resultAddr / 0x1000 != (resultAddr + size - 1) / 0x1000);

            return result;
        }

        public static bool ExpandHeap(bool print)
        {
            if (fblock == null)
            {
                // Creates initial heap of 40KiB
                if (!DoExpandHeap(0xA000))
                {
                    return false;
                }
                return true;
            }
            if (!fblock->expanding)
            {
                HeapBlock* oldFBlock = fblock;
                oldFBlock->expanding = true;
                // Expand by 1MiB
                if (!DoExpandHeap(0x100000))
                {
                    return false;
                }
                //else
                //{
                //    BasicConsole.WriteLine("Heap expanded successfully.");
                //    if (print)
                //    {
                //        BasicConsole.Write("New heap size: ");
                //        BasicConsole.Write(GetTotalMem() / 1024);
                //        BasicConsole.WriteLine("KiB");
                //    }
                //}
                oldFBlock->expanding = false;
                return true;
            }
            return false;
        }

        public static void Free(void* ptr)
        {
            EnterCritical("Free");

            HeapBlock* b;
            int ptroff;
            int bi, x;
            byte* bm;
            byte id;
            int max;

            for (b = fblock; (int)b != 0; b = b->next)
            {
                if ((int)ptr > (int)b && (int)ptr < (int)b + b->size)
                {
                    /* found block */
                    ptroff = (int)ptr - (int)&b[1]; /* get offset to get block */
                    /* block offset in BM */
                    bi = ptroff / b->bsize;
                    /* .. */
                    bm = (byte*)&b[1];
                    /* clear allocation */
                    id = bm[bi];
                    /* oddly.. HeapC did not optimize this */
                    max = b->size / b->bsize;
                    for (x = bi; bm[x] == id && x < max; ++x)
                    {
                        bm[x] = 0;
                    }
                    /* update free block count */
                    b->used -= x - bi;

                    ExitCritical();
                    return;
                }
            }

            /* this error needs to be raised or reported somehow */
            ExitCritical();
        }

        public static int GetFreeMem(HeapBlock* aBlock)
        {
            return aBlock->size - aBlock->used * aBlock->bsize;
        }

        public static byte GetNID(byte a, byte b)
        {
            byte c;
            for (c = (byte)(a + 1); c == b || c == 0; ++c) ;
            return c;
        }

        public static int GetTotalFreeMem()
        {
            HeapBlock* cBlock = fblock;
            int result = 0;
            while (cBlock != null)
            {
                result += GetFreeMem(cBlock);
                cBlock = cBlock->next;
            }
            return result;
        }

        public static int GetTotalMem()
        {
            HeapBlock* cBlock = fblock;
            int result = 0;
            while (cBlock != null)
            {
                result += cBlock->size;
                cBlock = cBlock->next;
            }
            return result;
        }

        public static int GetTotalUsedMem()
        {
            HeapBlock* cBlock = fblock;
            int result = 0;
            while (cBlock != null)
            {
                result += GetUsedMem(cBlock);
                cBlock = cBlock->next;
            }
            return result;
        }

        public static int GetUsedMem(HeapBlock* aBlock)
        {
            return aBlock->used * aBlock->bsize;
        }

        public static void Init()
        {
            // Initial heap creation
            ExpandHeap(false);

            AccessLock = new SpinLock();

            AccessLockInitialised = true;
        }

        public static int InitBlock(HeapBlock* b, int size, int bsize)
        {
            int bcnt;

            b->size = size - sizeof(HeapBlock);
            b->bsize = bsize;
            b->expanding = false;

            bcnt = size / bsize;
            byte* bm = (byte*)&b[1];

            /* clear bitmap */
            for (int x = 0; x < bcnt; ++x)
            {
                bm[x] = 0;
            }

            /* reserve room for bitmap */
            bcnt = bcnt / bsize * bsize < bcnt ? bcnt / bsize + 1 : bcnt / bsize;
            for (int x = 0; x < bcnt; ++x)
            {
                bm[x] = 5;
            }

            b->lfb = bcnt - 1;

            b->used = bcnt;
            b->next = null;

            return 1;
        }

        /// <summary>
        ///     Calculates the total amount of memory in the heap.
        /// </summary>
        /// <returns>The total amount of memory in the heap.</returns>
        /// <summary>
        ///     Calculates the total amount of used memory in the heap.
        /// </summary>
        /// <returns>The total amount of used memory in the heap.</returns>
        /// <summary>
        ///     Calculates the total amount of free memory in the heap.
        /// </summary>
        /// <returns>The total amount of free memory in the heap.</returns>
        /// <summary>
        ///     Calculates the amount of used memory in the specified block.
        /// </summary>
        /// <param name="aBlock">The block to calculate used mem of.</param>
        /// <returns>The amount of used memory in bytes.</returns>
        /// <summary>
        ///     Calculates the amount of free memory in the specified block.
        /// </summary>
        /// <param name="aBlock">The block to calculate free mem of.</param>
        /// <returns>The amount of free memory in bytes.</returns>
        public static void Load(HeapBlock* heapPtr, SpinLock heapLock)
        {
            fblock = heapPtr;
            AccessLock = heapLock;
            AccessLockInitialised = AccessLock != null;
        }

        /// <summary>
        ///     A pointer to the most-recently added heap block.
        /// </summary>
        private static HeapBlock* fblock = null;

        /// <summary>
        ///     Adds a contiguous block of memory to the heap so it can be used for allocating memory to objects.
        /// </summary>
        /// <returns>Returns 1 if the block was added successfully.</returns>
        /// <summary>
        ///     Don't understand what this actually does...anyone care to inform me?
        /// </summary>
        /// <param name="a">Umm...</param>
        /// <param name="b">Umm...</param>
        /// <returns>Umm...the NID I guess... :)</returns>
        /// <summary>
        ///     Attempts to allocate the specified amount of memory from the heap.
        /// </summary>
        /// <param name="size">The amount of memory to try and allocate.</param>
        /// <returns>
        ///     A pointer to the start of the allocated memory or a null pointer if not enough
        ///     contiguous memory is available.
        /// </returns>
        /// <summary>
        ///     Attempts to allocate the specified amount of memory from the heap and then zero all of it.
        /// </summary>
        /// <param name="size">The amount of memory to try and allocate.</param>
        /// <returns>
        ///     A pointer to the start of the allocated memory or a null pointer if not enough
        ///     contiguous memory is available.
        /// </returns>
        /// <summary>
        ///     Avoids Page Boundary.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        /// <summary>
        ///     Attempts to allocate the specified amount of memory from the heap and then zero all of it.
        /// </summary>
        /// <param name="size">The amount of memory to try and allocate.</param>
        /// <param name="boundary">The boundary on which the data must be allocated. 1 = no boundary. Must be power of 2.</param>
        /// <returns>
        ///     A pointer to the start of the allocated memory or a null pointer if not enough
        ///     contiguous memory is available.
        /// </returns>
        /// <summary>
        ///     Attempts to allocate the specified amount of memory from the heap.
        /// </summary>
        /// <param name="size">The amount of memory to try and allocate.</param>
        /// <param name="boundary">The boundary on which the data must be allocated. 1 = no boundary. Must be power of 2.</param>
        /// <returns>
        ///     A pointer to the start of the allocated memory or a null pointer if not enough
        ///     contiguous memory is available.
        /// </returns>
        /// <summary>
        ///     Frees the specified memory giving it back to the heap.
        /// </summary>
        /// <param name="ptr">A pointer to the memory to free.</param>
        /// <summary>
        ///     Intialises the heap.
        /// </summary>
        private static bool DoExpandHeap(int Size)
        {
            int NumPages = (Size + 4095) / 4096;
            int FinalSize = NumPages * 4096;
            int StartAddress = Marshal.AllocHGlobal(FinalSize).ToInt32();

            HeapBlock* NewBlockPtr = (HeapBlock*)StartAddress;
            InitBlock(NewBlockPtr, FinalSize, 32);
            AddBlock(NewBlockPtr);
            return true;
        }

        private static void EnterCritical(String caller)
        {
            //BasicConsole.WriteLine("Entering critical section...");
            if (AccessLockInitialised)
            {
                bool l = false; ;
                AccessLock?.Enter(ref l);
            }
        }

        private static void ExitCritical()
        {
            //BasicConsole.WriteLine("Exiting critical section...");
            if (AccessLockInitialised)
            {
                AccessLock?.Exit();
            }
        }
    }
}