using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Boards.Blocks;
using Unit.GameScene.Units.Blocks.Units.MatchBlock;

namespace Unit.GameScene.Boards.Interfaces
{
    /// <summary>
    ///     블록 생성기 인터페이스
    /// </summary>
    public interface IBlockGenerator
    {
        void GenerateAllRandomBlocks();
        BlockModel GetRandomValidBlock(Dictionary<Tuple<float, float>, MatchMatchBlockView> tiles, Tuple<float, float> pos);
    }
}