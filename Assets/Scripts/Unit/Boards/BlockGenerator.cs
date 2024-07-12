using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.Blocks;
using Unit.Boards.Interfaces;
using UnityEngine;

namespace Unit.Boards
{
    /// <summary>
    /// 블록을 생성하는 클래스입니다.
    /// </summary>
    public class BlockGenerator : IBlockGenerator
    {
        private readonly Tuple<float, float> _spawnPositionWidth;
        private readonly Tuple<float, float> _spawnPositionHeight;
        private float _blockOffset;
        private readonly List<Tuple<float, float>> _blockPositions;
        private readonly List<NewBlock> _blockInfos;
        private readonly Action<Vector3, Vector3> _matchCheckHandler;
        private readonly IBlockPool _blockPool;
        private readonly Dictionary<Tuple<float, float>, Block> _tiles;

        /// <summary>
        /// BlockGenerator 생성자입니다.
        /// </summary>
        /// <param name="spawnPositionWidth">블록 스폰 위치의 너비</param>
        /// <param name="spawnPositionHeight">블록 스폰 위치의 높이</param>
        /// <param name="blockInfos">블록 정보 목록</param>
        /// <param name="matchCheckHandler">매치 확인 핸들러</param>
        /// <param name="blockPool">블록 풀</param>
        /// <param name="tiles">블록 딕셔너리</param>
        public BlockGenerator(Tuple<float, float> spawnPositionWidth, Tuple<float, float> spawnPositionHeight, float blockOffset, List<NewBlock> blockInfos,
            Action<Vector3, Vector3> matchCheckHandler, IBlockPool blockPool, Dictionary<Tuple<float, float>, Block> tiles)
        {
            _spawnPositionWidth = spawnPositionWidth;
            _spawnPositionHeight = spawnPositionHeight;
            _blockOffset = blockOffset;
            _blockPositions = new List<Tuple<float, float>>();
            _blockInfos = blockInfos;
            _matchCheckHandler = matchCheckHandler;
            _blockPool = blockPool;
            _tiles = tiles;

            CalculateBlockPositions();
        }

        /// <summary>
        /// 블록 위치를 계산합니다.
        /// </summary>
        private void CalculateBlockPositions()
        {
            for (var x = _spawnPositionWidth.Item1; x <= _spawnPositionWidth.Item2; x += _blockOffset)
            {
                for (var y = _spawnPositionHeight.Item1 ; y <= _spawnPositionHeight.Item2; y += _blockOffset)
                {
                    Debug.Log($"_blockPositions {x} {y}");
                    _blockPositions.Add(new Tuple<float, float>(x, y));
                }
            }
        }

        /// <summary>
        /// 모든 블록을 랜덤하게 생성합니다. 파라미터를 통해 랜덤 블록 생성 시, 초기 매칭이 가능하도록 할지 여부를 결정할 수 있습니다.
        /// </summary>
        public void GenerateAllRandomBlocks()
        {
            _tiles.Clear();

            foreach (var blockPosition in _blockPositions)
            {
                var selectedBlockInfo = GetRandomValidBlock(_tiles, blockPosition);
                InstantiateAndAddBlock(blockPosition, selectedBlockInfo);
            }
        }

        /// <summary>
        /// 블록을 인스턴스화하고 딕셔너리에 추가합니다.
        /// </summary>
        /// <param name="blockPosition">블록의 위치</param>
        /// <param name="blockInfo">블록 정보</param>
        private void InstantiateAndAddBlock(Tuple<float, float> blockPosition, NewBlock blockInfo)
        {
            Vector3 position = new(blockPosition.Item1, blockPosition.Item2, 0);
            var block = _blockPool.Get();
            block.transform.localPosition = position;
            block.Initialize(blockInfo, _matchCheckHandler);
            _tiles.Add(blockPosition, block);
        }

        /// <summary>
        /// 주어진 위치에 대해 유효한 랜덤 블록을 가져옵니다.
        /// </summary>
        /// <param name="blockDic">블록 딕셔너리</param>
        /// <param name="position">블록의 위치</param>
        /// <returns>유효한 랜덤 블록</returns>
        public NewBlock GetRandomValidBlock(Dictionary<Tuple<float, float>, Block> blockDic, Tuple<float, float> position)
        {
            List<NewBlock> validBlocks = new();

            foreach (var blockInfo in _blockInfos)
            {
                if (IsValidPosition(blockDic, position, blockInfo))
                {
                    validBlocks.Add(blockInfo);
                }
            }

            if (validBlocks.Count == 0)
            {
                return _blockInfos[UnityEngine.Random.Range(0, _blockInfos.Count)];
            }

            return validBlocks[UnityEngine.Random.Range(0, validBlocks.Count)];
        }

        /// <summary>
        /// 주어진 위치에 블록을 배치할 수 있는지 확인합니다.
        /// </summary>
        /// <param name="blockDic">블록 딕셔너리</param>
        /// <param name="position">블록의 위치</param>
        /// <param name="blockInfo">블록 정보</param>
        /// <returns>유효한 위치 여부</returns>
        private bool IsValidPosition(Dictionary<Tuple<float, float>, Block> blockDic, Tuple<float, float> position, NewBlock blockInfo)
        {
            var directions = new[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            foreach (var direction in directions)
            {
                if (CheckDirection(blockDic, position, blockInfo, direction) >= 2)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 주어진 방향으로 몇 개의 블록이 매칭되는지 확인합니다.
        /// </summary>
        /// <param name="blockDic">블록 딕셔너리</param>
        /// <param name="position">블록의 위치</param>
        /// <param name="blockInfo">블록 정보</param>
        /// <param name="direction">검사할 방향</param>
        /// <returns>매칭되는 블록 수</returns>
        private int CheckDirection(Dictionary<Tuple<float, float>, Block> blockDic, Tuple<float, float> position, NewBlock blockInfo, Vector2 direction)
        {
            var matchCount = 0;
            Tuple<float, float> currentPos = new(position.Item1 + direction.x, position.Item2 + direction.y);

            while (blockDic.ContainsKey(currentPos) && blockDic[currentPos].Type == blockInfo.type)
            {
                matchCount++;
                currentPos = new Tuple<float, float>(currentPos.Item1 + direction.x, currentPos.Item2 + direction.y);
            }

            return matchCount;
        }
    }
}
