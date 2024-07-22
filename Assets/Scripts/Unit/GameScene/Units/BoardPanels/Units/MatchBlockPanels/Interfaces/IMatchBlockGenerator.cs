using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Units.Blocks.Units.MatchBlock;

namespace Unit.GameScene.Units.BoardPanels.Units.MatchBlockPanels.Interfaces
{
    /// <summary>
    ///     블록 생성기 인터페이스
    /// </summary>
    public interface IMatchBlockGenerator
    {
        void GenerateAllRandomBlocks();
        BlockModel GetRandomValidBlock(Dictionary<Tuple<float, float>, MatchBlockView> tiles, Tuple<float, float> pos);
    }
}