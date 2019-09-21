using System;
using System.Collections.Generic;
using System.IO;

namespace Ref.Core
{
    public class Heap
    {
        public Heap()
        {
            this.storage = new BlockStorage(new MemoryStream());

            if (storage.BlockHeaderSize < 48)
            {
                throw new ArgumentException("Record storage needs at least 48 header bytes");
            }
        }

        public Addr Allocate(int size)
        {
            var firstBlock = AllocateBlock();

            return new Addr(firstBlock.Id, this);
        }

        public void Free(Addr address)
        {
            var block = storage.Find(address.ToUInt());

            Block currentBlock = block;
            while (true)
            {
                Block nextBlock = null;

                MarkAsFree(currentBlock.Id);
                currentBlock.SetHeader(kIsDeleted, 1L);

                var nextBlockId = (uint)currentBlock.GetHeader(kNextBlockId);
                if (nextBlockId == 0)
                {
                    break;
                }
                else
                {
                    nextBlock = storage.Find(nextBlockId);
                    if (currentBlock == null)
                    {
                        throw new InvalidDataException("Block not found by id: " + nextBlockId);
                    }
                }

                // Move to next block
                if (nextBlock != null)
                {
                    currentBlock = nextBlock;
                }
            }
        }

        internal readonly BlockStorage storage;

        private const int kBlockContentLength = 2;

        private const int kIsDeleted = 4;

        private const int kNextBlockId = 0;

        private const int kPreviousBlockId = 3;

        private const int kRecordLength = 1;

        private const int MaxRecordSize = 4194304;

        private Block AllocateBlock()
        {
            uint resuableBlockId;
            Block newBlock;
            if (false == TryFindFreeBlock(out resuableBlockId))
            {
                newBlock = storage.CreateNew();
                if (newBlock == null)
                {
                    throw new Exception("Failed to create new block");
                }
            }
            else
            {
                newBlock = storage.Find(resuableBlockId);
                if (newBlock == null)
                {
                    throw new InvalidDataException("Block not found by id: " + resuableBlockId);
                }
                newBlock.SetHeader(kBlockContentLength, 0L);
                newBlock.SetHeader(kNextBlockId, 0L);
                newBlock.SetHeader(kPreviousBlockId, 0L);
                newBlock.SetHeader(kRecordLength, 0L);
                newBlock.SetHeader(kIsDeleted, 0L);
            }
            return newBlock;
        }

        private void AppendUInt32ToContent(Block block, uint value)
        {
            var contentLength = block.GetHeader(kBlockContentLength);

            if ((contentLength % 4) != 0)
            {
                throw new DataMisalignedException("Block content length not %4: " + contentLength);
            }

            block.Write(src: LittleEndianByteOrder.GetBytes(value), srcOffset: 0, dstOffset: (int)contentLength, count: 4);
        }

        private List<Block> FindBlocks(uint recordId)
        {
            var blocks = new List<Block>();
            var success = false;

            try
            {
                var currentBlockId = recordId;

                do
                {
                    // Grab next block
                    var block = storage.Find(currentBlockId);
                    if (null == block)
                    {
                        // Special case: if block #0 never created, then attempt to create it
                        if (currentBlockId == 0)
                        {
                            block = storage.CreateNew();
                        }
                        else
                        {
                            throw new Exception("Block not found by id: " + currentBlockId);
                        }
                    }
                    blocks.Add(block);

                    // If this is a deleted block then ignore the fuck out of it
                    if (1L == block.GetHeader(kIsDeleted))
                    {
                        throw new InvalidDataException("Block not found: " + currentBlockId);
                    }

                    // Move next
                    currentBlockId = (uint)block.GetHeader(kNextBlockId);
                } while (currentBlockId != 0);

                success = true;
                return blocks;
            }
            finally
            {
                // Incase shit happens, dispose all fetched blocks
                if (false == success)
                {
                    foreach (var block in blocks)
                    {
                        block.Dispose();
                    }
                }
            }
        }

        private void GetSpaceTrackingBlock(out Block lastBlock, out Block secondLastBlock)
        {
            lastBlock = null;
            secondLastBlock = null;

            // Grab all record 0's blocks
            var blocks = FindBlocks(0);

            try
            {
                if (blocks == null || (blocks.Count == 0))
                {
                    throw new Exception("Failed to find blocks of record 0");
                }

                // Assign
                lastBlock = blocks[blocks.Count - 1];
                if (blocks.Count > 1)
                {
                    secondLastBlock = blocks[blocks.Count - 2];
                }
            }
            finally
            {
                // Awlays dispose unused blocks
                if (blocks != null)
                {
                    foreach (var block in blocks)
                    {
                        if ((lastBlock == null || block != lastBlock)
                            && (secondLastBlock == null || block != secondLastBlock))
                        {
                            block.Dispose();
                        }
                    }
                }
            }
        }

        private void MarkAsFree(uint blockId)
        {
            Block lastBlock, secondLastBlock, targetBlock = null;
            GetSpaceTrackingBlock(out lastBlock, out secondLastBlock);

            try
            {
                // Just append a number, if there is some space left
                var contentLength = lastBlock.GetHeader(kBlockContentLength);
                if ((contentLength + 4) <= storage.BlockContentSize)
                {
                    targetBlock = lastBlock;
                }
                // No more fucking space left, allocate new block for writing.
                // Note that we allocate fresh new block, if we reuse it may fuck things up
                else
                {
                    targetBlock = storage.CreateNew();
                    targetBlock.SetHeader(kPreviousBlockId, lastBlock.Id);

                    lastBlock.SetHeader(kNextBlockId, targetBlock.Id);

                    contentLength = 0;
                }

                // Write!
                AppendUInt32ToContent(targetBlock, blockId);

                // Extend the block length to 4, as we wrote a number
                targetBlock.SetHeader(kBlockContentLength, contentLength + 4);
            }
            finally
            {
                // Always dispose targetBlock
                if (targetBlock != null)
                {
                    targetBlock.Dispose();
                }
            }
        }

        private uint ReadUInt32FromTrailingContent(Block block)
        {
            var buffer = new byte[4];
            var contentLength = block.GetHeader(kBlockContentLength);

            if ((contentLength % 4) != 0)
            {
                throw new DataMisalignedException("Block content length not %4: " + contentLength);
            }

            if (contentLength == 0)
            {
                throw new InvalidDataException("Trying to dequeue UInt32 from an empty block");
            }

            block.Read(dest: buffer, destOffset: 0, srcOffset: (int)contentLength - 4, count: 4);
            return LittleEndianByteOrder.GetUInt32(buffer);
        }

        private bool TryFindFreeBlock(out uint blockId)
        {
            blockId = 0;
            Block lastBlock, secondLastBlock;
            GetSpaceTrackingBlock(out lastBlock, out secondLastBlock);

            // If this block is empty, then goto previous block
            var currentBlockContentLength = lastBlock.GetHeader(kBlockContentLength);
            if (currentBlockContentLength == 0)
            {
                // If there is no previous block, return false to indicate we can't dequeu
                if (secondLastBlock == null)
                {
                    return false;
                }

                // Dequeue an uint from previous block, then mark current block as free
                blockId = ReadUInt32FromTrailingContent(secondLastBlock);

                // Back off 4 bytes before calling AppendUInt32ToContent
                secondLastBlock.SetHeader(kBlockContentLength, secondLastBlock.GetHeader(kBlockContentLength) - 4);
                AppendUInt32ToContent(secondLastBlock, lastBlock.Id);

                // Forward 4 bytes, as an uint32 has been written
                secondLastBlock.SetHeader(kBlockContentLength, secondLastBlock.GetHeader(kBlockContentLength) + 4);
                secondLastBlock.SetHeader(kNextBlockId, 0);
                lastBlock.SetHeader(kPreviousBlockId, 0);

                // Indicate success
                return true;
            }
            // If this block is not empty then dequeue an UInt32 from it
            else
            {
                blockId = ReadUInt32FromTrailingContent(lastBlock);
                lastBlock.SetHeader(kBlockContentLength, currentBlockContentLength - 4);

                // Indicate sucess
                return true;
            }
        }
    }
}