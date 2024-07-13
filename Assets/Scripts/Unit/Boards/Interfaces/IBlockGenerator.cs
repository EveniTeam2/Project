using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.Boards.Blocks;

namespace Unit.Boards.Interfaces
{
    public interface IBlockGenerator
    {
        void GenerateAllRandomBlocks();
        NewBlock GetRandomValidBlock(Dictionary<Tuple<float, float>, Block> tiles, Tuple<float, float> pos);
    }
}