using System;
using System.Collections.Generic;
using Unit.GameScene.Boards.Blocks;
using Unit.GameScene.Units.Blocks.Units.MatchBlock;
using UnityEngine;

namespace Unit.GameScene.Boards.Interfaces
{
    /// <summary>
    ///     블록 매치 검사 인터페이스
    /// </summary>
    public interface IBlockMatcher
    {
        Tuple<float, float> GetTargetIndex(Vector2 startPosition, Vector2 direction);
        bool IsValidPosition(Tuple<float, float> position);
        bool CheckMatchesForBlock(Tuple<float, float> position, out List<MatchMatchBlockView> matchedBlocks);
        List<MatchMatchBlockView> GetAdjacentMatches(List<MatchMatchBlockView> initialMatches);
        List<MatchMatchBlockView> FindAllMatches(Dictionary<Tuple<float, float>, MatchMatchBlockView> tiles);
    }
}