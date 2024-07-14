using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.Boards.Blocks;
using UnityEngine;

namespace Unit.Boards.Interfaces
{
    /// <summary>
    /// 블록 생성기 인터페이스
    /// </summary>
    public interface IBlockGenerator
    {
        void GenerateAllRandomBlocks();
        BlockSo GetRandomValidBlock(Dictionary<Tuple<float, float>, Block> tiles, Tuple<float, float> pos);
    }
}