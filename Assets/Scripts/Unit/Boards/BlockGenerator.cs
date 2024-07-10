using System;
using System.Collections.Generic;
using Core.Utils;
using ScriptableObjects.Scripts.Blocks;
using Unit.Blocks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit.Boards
{
    public class BlockGenerator
    {
        private readonly float _spawnPositionWidth;
        private readonly float _spawnPositionHeight;

        private readonly List<Tuple<float, float>> _blockPositions;
        private readonly List<NewBlock> _blockInfos;

        private readonly Action<Vector3, Vector3> _matchCheckHandler;
        private readonly BlockPool _blockPool;

        public BlockGenerator(float spawnPositionWidth, float spawnPositionHeight, List<NewBlock> blockInfos,
            Action<Vector3, Vector3> matchCheckHandler, BlockPool blockPool,
            out SerializableDictionary<Tuple<float, float>, Block> tiles)
        {
            _spawnPositionWidth = spawnPositionWidth;
            _spawnPositionHeight = spawnPositionHeight;
            _blockPositions = new List<Tuple<float, float>>();
            _blockInfos = blockInfos;
            _matchCheckHandler = matchCheckHandler;
            _blockPool = blockPool;

            CalculateBlockPositions();
            
            tiles = GenerateAllBlocks();
        }

        private void CalculateBlockPositions()
        {
            for (var x = -_spawnPositionHeight; x <= _spawnPositionHeight; x++)
            {
                for (var y = -_spawnPositionWidth; y <= _spawnPositionWidth; y++)
                {
                    _blockPositions.Add(new Tuple<float, float>(x, y));
                }
            }
        }

        private SerializableDictionary<Tuple<float, float>, Block> GenerateAllBlocks()
        {
            var blockDic = new SerializableDictionary<Tuple<float, float>, Block>();

            foreach (var blockPosition in _blockPositions)
            {
                var selectedBlockInfo = GetRandomValidBlock(blockDic, blockPosition);

                InstantiateAndAddBlock(blockDic, blockPosition, selectedBlockInfo);
            }

            return blockDic;
        }

        private void InstantiateAndAddBlock(SerializableDictionary<Tuple<float, float>, Block> blockDic,
            Tuple<float, float> blockPosition, NewBlock blockInfo)
        {
            Vector3 position = new(blockPosition.Item1, blockPosition.Item2, 0);
            var block = _blockPool.Get();
            block.transform.position = position;
            block.Initialize(blockInfo, _matchCheckHandler);
            blockDic.Add(blockPosition, block);
        }

        public NewBlock GetRandomValidBlock(SerializableDictionary<Tuple<float, float>, Block> blockDic, Tuple<float, float> position)
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
                return _blockInfos[Random.Range(0, _blockInfos.Count)];
            }

            return validBlocks[Random.Range(0, validBlocks.Count)];
        }

        private bool IsValidPosition(SerializableDictionary<Tuple<float, float>, Block> blockDic, Tuple<float, float> position, NewBlock blockInfo)
        {
            var directions = new[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            foreach (var direction in directions)
            {
                if (CheckDirection(blockDic, position, blockInfo, direction) >= 3 - 1)
                {
                    return false;
                }
            }

            return true;
        }

        private int CheckDirection(SerializableDictionary<Tuple<float, float>, Block> blockDic, Tuple<float, float> position, NewBlock blockInfo, Vector2 direction)
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