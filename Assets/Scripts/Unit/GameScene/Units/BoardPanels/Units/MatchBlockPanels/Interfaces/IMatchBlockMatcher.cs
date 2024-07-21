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
    public interface IMatchBlockMatcher
    {
        Tuple<float, float> GetTargetIndex(Vector2 startPosition, Vector2 direction);
        bool IsValidPosition(Tuple<float, float> position);
        bool CheckMatchesForBlock(Tuple<float, float> position, out List<MatchBlockView> matchedBlocks);
        List<MatchBlockView> GetAdjacentMatches(List<MatchBlockView> initialMatches);
        List<MatchBlockView> FindAllMatches(Dictionary<Tuple<float, float>, MatchBlockView> tiles);
    }
}