using System;
using System.Collections;
using Unit.Boards.Blocks;

namespace Unit.Boards.Interfaces
{
    /// <summary>
    /// 블록 이동 처리 인터페이스
    /// </summary>
    public interface IBlockMover
    {
        IEnumerator SwapBlock(Block currentBlock, Block targetBlock, Tuple<float, float> currentPos, Tuple<float, float> targetPos);
        IEnumerator DropBlock(Tuple<float, float> targetPos, Block currentBlock);
    }
}