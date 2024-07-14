using System;
using System.Collections.Generic;
using Unit.GameScene.Boards.Blocks;
using UnityEngine;

namespace Unit.GameScene.Boards.Interfaces
{
    /// <summary>
    /// 블록 매치 검사 인터페이스
    /// </summary>
    public interface IBlockMatcher
    {
        bool CheckMatchesForBlock(Tuple<float, float> position, out List<Block> matchedBlocks);
        List<Block> GetAdjacentMatches(List<Block> initialMatches);
        List<Block> FindAllMatches(Dictionary<Tuple<float, float>, Block> tiles);
        Tuple<float, float> GetTargetIndex(Vector2 startPosition, Vector2 direction);
        bool IsValidPosition(Tuple<float, float> position);
    }
}