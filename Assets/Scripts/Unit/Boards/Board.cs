using System;
using System.Collections.Generic;
using Manager;
using Unit.Blocks;
using UnityEngine;

namespace Unit.Boards
{
    public class Board : MonoBehaviour
    {
        [Header("블록 이동 완료까지 걸리는 시간 (단위 : 초)")]
        [SerializeField] private float duration = 0.5f;

        [Header("블록 풀링 관련 설정")]
        [SerializeField] private Block blockPrefab;
        [SerializeField] private int poolSize = 100;

        private int _width;
        private int _height;

        private BlockGenerator _blockGenerator;
        private BlockMatcher _blockMatcher;
        private BlockMover _blockMover;
        private BlockPool _blockPool;

        private Dictionary<Tuple<float, float>, Block> _tiles;

        private void Awake()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            _width = GameManager.Instance.boardWidth;
            _height = GameManager.Instance.boardHeight;

            _blockPool = new BlockPool(blockPrefab, transform, poolSize, true);
            _blockGenerator = new BlockGenerator(_width, _height, GameManager.Instance.blockInfos, CheckForMatch, _blockPool);
            _tiles = _blockGenerator.GenerateAllBlocks();
            _blockMatcher = new BlockMatcher(_tiles);
            _blockMover = new BlockMover(duration);
        }

        private void CheckForMatch(Vector3 startPosition, Vector3 direction)
        {
            var currentBlockIndex = new Tuple<float, float>(startPosition.x, startPosition.y);
            var targetBlockIndex = _blockMatcher.GetTargetIndex(startPosition, direction);

            Debug.Log($"블록 {startPosition} 이 {targetBlockIndex} 위치로 이동했을 때, 매치 검증");

            if (_blockMatcher.IsValidPosition(targetBlockIndex))
            {
                Debug.Log("유효한 위치");

                var currentBlock = _tiles[currentBlockIndex];
                var targetBlock = _tiles[targetBlockIndex];

                _tiles[currentBlockIndex] = targetBlock;
                _tiles[targetBlockIndex] = currentBlock;

                var targetHasMatches = _blockMatcher.CheckMatchesForBlock(targetBlockIndex, out var targetMatchedBlocks);
                var currentHasMatches = _blockMatcher.CheckMatchesForBlock(currentBlockIndex, out var currentMatchedBlocks);

                if (targetHasMatches || currentHasMatches)
                {
                    Debug.Log("매치되는 블록 확인, 타일 이동 코루틴 실행");

                    StartCoroutine(_blockMover.MoveBlock(currentBlock, targetBlock, targetBlockIndex, currentBlockIndex, () =>
                    {
                        OnMoveComplete(targetMatchedBlocks, currentMatchedBlocks);
                    }));
                }
                else
                {
                    Debug.Log("매치되는 블록 없음");

                    _tiles[currentBlockIndex] = currentBlock;
                    _tiles[targetBlockIndex] = targetBlock;
                }
            }
            else
            {
                Debug.Log("유효하지 않은 위치");
            }
        }

        private void OnMoveComplete(List<Block> targetMatchedBlocks, List<Block> currentMatchedBlocks)
        {
            if (targetMatchedBlocks.Count > 0)
            {
                RemoveMatchedBlocks(targetMatchedBlocks);
            }

            if (currentMatchedBlocks.Count > 0)
            {
                RemoveMatchedBlocks(currentMatchedBlocks);
            }
        }

        private void RemoveMatchedBlocks(List<Block> matchedBlocks)
        {
            foreach (var block in matchedBlocks)
            {
                var blockPos = block.transform.position;
                _tiles.Remove(new Tuple<float, float>(blockPos.x, blockPos.y));
                _blockPool.Release(block);
            }
        }
    }
}