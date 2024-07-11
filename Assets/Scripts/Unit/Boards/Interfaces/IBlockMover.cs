using System;
using System.Collections;
using Unit.Blocks;

namespace Unit.Boards.Interfaces
{
    public interface IBlockMover
    {
        IEnumerator SwapBlock(Block currentBlock, Block targetBlock, Tuple<float, float> currentPos, Tuple<float, float> targetPos);
        IEnumerator DropBlock(Tuple<float, float> targetPos, Block currentBlock);
    }
}