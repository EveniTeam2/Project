using System;
using System.Collections;
using Unit.GameScene.Boards.Blocks;
using Unit.GameScene.Units.Blocks.Abstract;
using Unit.GameScene.Units.Blocks.Units.MatchBlock;

namespace Unit.GameScene.Boards.Interfaces
{
    /// <summary>
    ///     블록 이동 처리 인터페이스
    /// </summary>
    public interface IMatchBlockMover
    {
        IEnumerator SwapBlock(BlockView currentBlockView, BlockView targetBlockView, Tuple<float, float> currentPos,
            Tuple<float, float> targetPos);

        IEnumerator DropBlock(Tuple<float, float> targetPos, BlockView currentBlockView);
    }
}