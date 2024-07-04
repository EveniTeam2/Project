using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.Blocks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit
{
    public class BlockGenerator : MonoBehaviour
    {
        private int _width;
        private int _height;

        private readonly List<Tuple<float, float>> _blockPositions = new();

        private const float BlockOffset = 0.5f;

        public void Initialize(int width, int height)
        {
            _width = width;
            _height = height;
            
            CalculateBlockPositions();
        }

        private void CalculateBlockPositions()
        {
            float adjustWidth = _width % 2 == 0 ? BlockOffset : 0;
            float adjustHeight = _height % 2 == 0 ? BlockOffset : 0;

            float halfWidth = _width / 2f - adjustWidth;
            float halfHeight = _height / 2f - adjustHeight;

            for (float x = -halfHeight; x <= halfHeight; x++)
            {
                for (float y = -halfWidth; y <= halfWidth; y++)
                {
                    _blockPositions.Add(new Tuple<float, float>(x, y));
                }
            }
        }

        public Dictionary<Tuple<float, float>, GameObject> GenerateAllBlocks(GameObject blockPrefab, IReadOnlyList<NewBlock> blockInfos, bool mergeLock)
        {
            Dictionary<Tuple<float, float>, GameObject> blockDic = new();

            foreach (var blockPosition in _blockPositions)
            {
                NewBlock selectedBlockInfo = GetRandomValidBlock(blockDic, blockPosition, blockInfos);
                
                InstantiateAndAddBlock(blockDic, blockPrefab, blockPosition, selectedBlockInfo);
            }

            return blockDic;
        }

        private void InstantiateAndAddBlock(Dictionary<Tuple<float, float>, GameObject> blockDic, GameObject blockPrefab, Tuple<float, float> blockPosition, NewBlock blockInfo)
        {
            Vector3 position = new(blockPosition.Item1, blockPosition.Item2, 0);
            
            var block = Instantiate(blockPrefab, position, Quaternion.identity);
            
            block.GetComponent<Block>().Initialize(blockInfo);
            blockDic.Add(blockPosition, block);
        }

        private NewBlock GetRandomValidBlock(Dictionary<Tuple<float, float>, GameObject> blockDic, Tuple<float, float> position, IReadOnlyList<NewBlock> blockInfos)
        {
            List<NewBlock> validBlocks = new();

            foreach (var blockInfo in blockInfos)
            {
                if (IsValidPosition(blockDic, position, blockInfo))
                {
                    validBlocks.Add(blockInfo);
                }
            }

            if (validBlocks.Count == 0)
            {
                return blockInfos[Random.Range(0, blockInfos.Count)];
            }

            return validBlocks[Random.Range(0, validBlocks.Count)];
        }

        private bool IsValidPosition(Dictionary<Tuple<float, float>, GameObject> blockDic, Tuple<float, float> position, NewBlock blockInfo)
        {
            return !(IsMatchingInDirection(blockDic, position, blockInfo, Vector2.up, Vector2.down) ||
                     IsMatchingInDirection(blockDic, position, blockInfo, Vector2.left, Vector2.right));
        }

        private bool IsMatchingInDirection(Dictionary<Tuple<float, float>, GameObject> blockDic, Tuple<float, float> position, NewBlock blockInfo, Vector2 dir1, Vector2 dir2)
        {
            return CheckDirection(blockDic, position, blockInfo, dir1) + CheckDirection(blockDic, position, blockInfo, dir2) >= 2;
        }

        private int CheckDirection(Dictionary<Tuple<float, float>, GameObject> blockDic, Tuple<float, float> position, NewBlock blockInfo, Vector2 direction)
        {
            int matchCount = 0;
            Tuple<float, float> currentPos = new(position.Item1 + direction.x, position.Item2 + direction.y);

            while (blockDic.ContainsKey(currentPos) && blockDic[currentPos].GetComponent<Block>().Type == blockInfo.type)
            {
                matchCount++;
                currentPos = new Tuple<float, float>(currentPos.Item1 + direction.x, currentPos.Item2 + direction.y);
            }

            return matchCount;
        }
    }
}