using System;
using System.Collections.Generic;
using Unit.Boards.Blocks;
using UnityEngine;

namespace Unit.Boards.Interfaces
{
    public interface IBlockMatcher
    {
        bool CheckMatchesForBlock(Tuple<float, float> position, out List<Block> matchedBlocks);
        List<Block> GetAdjacentMatches(List<Block> initialMatches);
        List<Block> FindAllMatches(Dictionary<Tuple<float, float>, Block> tiles);
        Tuple<float, float> GetTargetIndex(Vector3 startPosition, Vector3 direction);
        bool IsValidPosition(Tuple<float, float> position);
    }
}