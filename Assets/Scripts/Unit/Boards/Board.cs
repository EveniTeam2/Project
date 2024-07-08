using System;
using System.Collections.Generic;
using Manager;
using ScriptableObjects.Scripts.Blocks;
using UnityEngine;

namespace Unit.Boards
{
    public class Board : MonoBehaviour
    {
        [Header("블록 이동 완료까지 걸리는 시간 (단위 : 초)")]
        [SerializeField] private float duration = 0.5f;

        private int _width;
        private int _height;

        private BlockGenerator _blockGenerator;
        private BlockMatcher _blockMatcher;
        private BlockMover _blockMover;

        private GameObject _blockPrefab;
        private List<NewBlock> _blockInfos;
        private Dictionary<Tuple<float, float>, GameObject> _tiles;

        private void Awake()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            _width = GameManager.Instance.boardWidth;
            _height = GameManager.Instance.boardHeight;
            _blockInfos = GameManager.Instance.blockInfos;
            _blockPrefab = GameManager.Instance.tilePrefab;

            _blockGenerator = new BlockGenerator(_width, _height, _blockInfos, CheckForMatch, out _tiles, _blockPrefab);
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

                if (_blockMatcher.CheckDirection(targetBlockIndex, Vector2.up, Vector2.down) ||
                    _blockMatcher.CheckDirection(targetBlockIndex, Vector2.left, Vector2.right))
                {
                    Debug.Log("매치되는 블록 확인, 타일 이동 중");
                    StartCoroutine(_blockMover.MoveBlock(currentBlock, targetBlock, targetBlockIndex, currentBlockIndex));
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
    }
}
