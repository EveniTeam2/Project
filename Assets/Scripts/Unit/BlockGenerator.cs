using System;
using System.Collections.Generic;
using Unit.Blocks;
using UnityEngine;

namespace Unit
{
    public class BlockGenerator
    {
        private int _width;
        private int _height;
        
        public BlockGenerator(int width, int height)
        {
            _width = width;
            _height = height;
        }

        /// <summary>
        /// 랜덤한 타일을 생성합니다.
        /// </summary>
        /// <param name="blockPrefab">blockPrefab 게임 오브젝트</param>
        /// <param name="mergeLock">true - 초반 머지가 불가능하도록 생성합니다. / false - 초반 머지가 가능하도록 생성합니다.</param>
        public Block[,] GenerateBlocks(GameObject blockPrefab, bool mergeLock)
        {
            var blocksPositions = new Tuple<int, int>[_width * _height];

            CalculateEachBlockPosition();
        }

        private void CalculateEachBlockPosition()
        {
            throw new NotImplementedException();
        }
    }
}