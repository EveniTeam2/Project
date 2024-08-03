using ScriptableObjects.Scripts.Blocks;
using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Blocks.UI;

namespace Unit.GameScene.Units.Panels.Interfaces
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