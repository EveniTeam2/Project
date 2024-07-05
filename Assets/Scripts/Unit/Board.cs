using System;
using System.Collections.Generic;
using Manager;
using ScriptableObjects.Scripts.Blocks;
using Unit.Blocks;
using UnityEngine;

namespace Unit
{
    public class Board : MonoBehaviour
    {
        private int _width;
        private int _height;
        
        private List<NewBlock> _blockInfos;
        private BlockGenerator _blockGenerator;
        private GameObject _blockPrefab;
        
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
            
            _tiles = new Dictionary<Tuple<float, float>, GameObject>();
            
            _blockGenerator = GetComponent<BlockGenerator>();
            _blockGenerator.Initialize(_width, _height, CheckForMatch);
            
            _tiles = _blockGenerator.GenerateAllBlocks(_blockPrefab, _blockInfos, true);
        }

        public void CheckForMatch(Vector3 position)
        {
            var blockIndex = GetBlockIndex(position);
            if (blockIndex == null) return;

            if (CheckDirection(blockIndex, Vector2.up, Vector2.down) ||
                CheckDirection(blockIndex, Vector2.left, Vector2.right))
            {
                Debug.Log("Match Found!");
                // 연속된 블록이 3개 이상인 경우 처리 로직
            }
            else
            {
                Debug.Log("No Match");
            }
        }

        private Tuple<int, int> GetBlockIndex(Vector3 position)
        {
            foreach (var tile in _tiles)
            {
                if (Vector3.Distance(tile.Value.transform.position, position) < 0.1f)
                {
                    return new Tuple<int, int>((int)tile.Key.Item1, (int)tile.Key.Item2);
                }
            }
            return null;
        }

        private bool CheckDirection(Tuple<int, int> start, Vector2 dir1, Vector2 dir2)
        {
            var matchCount = 1; // 자신 포함

            matchCount += CountMatchesInDirection(start, dir1);
            matchCount += CountMatchesInDirection(start, dir2);

            return matchCount >= 3;
        }

        private int CountMatchesInDirection(Tuple<int, int> start, Vector2 direction)
        {
            var matchCount = 0;
            var x = start.Item1;
            var y = start.Item2;

            while (true)
            {
                x += (int)direction.x;
                y += (int)direction.y;

                if (!_tiles.ContainsKey(new Tuple<float, float>(x, y)) ||
                    _tiles[new Tuple<float, float>(x, y)].GetComponent<Block>().Type !=
                    _tiles[new Tuple<float, float>(start.Item1, start.Item2)].GetComponent<Block>().Type)
                {
                    break;
                }

                matchCount++;
            }

            return matchCount;
        }
    }
}
