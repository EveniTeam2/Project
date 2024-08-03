using System;
using System.Collections;
using Unit.GameScene.Units.Blocks.Abstract;

namespace Unit.GameScene.Units.Panels.Interfaces
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