using System;
using System.Collections.Generic;
using Unit.Boards.Blocks;
using Unit.Boards.Interfaces;
using UnityEngine;

namespace Unit.Boards
{
    /// <summary>
    /// 블록의 매칭을 검사하는 클래스입니다.
    /// </summary>
    public class BlockMatcher : IBlockMatcher
    {
        private readonly Dictionary<Tuple<float, float>, Block> _tiles;
        private readonly float _blockGap;

        public BlockMatcher(Dictionary<Tuple<float, float>, Block> tiles, float blockGap)
        {
            _tiles = tiles;
            _blockGap = blockGap;
        }

        /// <summary>
        /// 블록이 이동할 목표 위치를 계산합니다.
        /// </summary>
        /// <param name="startPosition">블록의 시작 위치</param>
        /// <param name="direction">블록이 이동할 방향</param>
        /// <returns>블록이 이동할 목표 위치</returns>
        public Tuple<float, float> GetTargetIndex(Vector2 startPosition, Vector2 direction)
        {
            var targetX = direction.x switch
            {
                > 0 => startPosition.x + _blockGap,
                < 0 => startPosition.x - _blockGap,
                _ => startPosition.x
            };

            var targetY = direction.y switch
            {
                > 0 => startPosition.y + _blockGap,
                < 0 => startPosition.y - _blockGap,
                _ => startPosition.y
            };

            return new Tuple<float, float>(targetX, targetY);
        }

        /// <summary>
        /// 주어진 위치가 유효한 위치인지 검사합니다.
        /// </summary>
        /// <param name="position">검사할 위치</param>
        /// <returns>유효한 위치 여부</returns>
        public bool IsValidPosition(Tuple<float, float> position)
        {
            return _tiles.ContainsKey(position);
        }

        /// <summary>
        /// 블록이 주어진 위치로 이동했을 때 매칭을 검사합니다.
        /// </summary>
        /// <param name="position">블록의 위치</param>
        /// <param name="matchedBlocks">매칭된 블록 목록</param>
        /// <returns>매칭이 있는지 여부</returns>
        public bool CheckMatchesForBlock(Tuple<float, float> position, out List<Block> matchedBlocks)
        {
            matchedBlocks = new List<Block>();

            if (CheckDirection(position, Vector2.up * _blockGap, Vector2.down * _blockGap, out var verticalMatches))
            {
                matchedBlocks.AddRange(verticalMatches);
            }

            if (CheckDirection(position, Vector2.left * _blockGap, Vector2.right * _blockGap, out var horizontalMatches))
            {
                matchedBlocks.AddRange(horizontalMatches);
            }

            if (matchedBlocks.Count > 0)
            {
                matchedBlocks.Add(_tiles[position]);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 주어진 방향으로 매칭을 검사합니다.
        /// </summary>
        /// <param name="start">시작 위치</param>
        /// <param name="dir1">첫 번째 방향</param>
        /// <param name="dir2">두 번째 방향</param>
        /// <param name="matches">매칭된 블록 목록</param>
        /// <returns>매칭이 있는지 여부</returns>
        private bool CheckDirection(Tuple<float, float> start, Vector2 dir1, Vector2 dir2, out List<Block> matches)
        {
            matches = new List<Block>();
            matches.AddRange(CountMatchesInDirection(start, dir1));
            matches.AddRange(CountMatchesInDirection(start, dir2));

            return matches.Count >= 2;
        }

        /// <summary>
        /// 주어진 방향으로 몇 개의 블록이 매칭되는지 셉니다.
        /// </summary>
        /// <param name="start">시작 위치</param>
        /// <param name="direction">매칭을 확인할 방향</param>
        /// <returns>매칭된 블록 목록</returns>
        private List<Block> CountMatchesInDirection(Tuple<float, float> start, Vector2 direction)
        {
            var matches = new List<Block>();
            var x = start.Item1;
            var y = start.Item2;

            while (true)
            {
                x += (int)direction.x;
                y += (int)direction.y;

                var pos = new Tuple<float, float>(x, y);
                if (!_tiles.ContainsKey(pos) || _tiles[pos].Type != _tiles[start].Type)
                {
                    break;
                }

                matches.Add(_tiles[pos]);
            }

            return matches;
        }

        /// <summary>
        /// 초기 매칭된 블록들의 인접 블록도 매칭된 블록으로 추가합니다.
        /// </summary>
        /// <param name="initialMatches">초기 매칭된 블록 목록</param>
        /// <returns>인접한 매칭된 블록 목록</returns>
        public List<Block> GetAdjacentMatches(List<Block> initialMatches)
        {
            Debug.Log($"인접한 동일 타입 블록 체크, 현재 {initialMatches.Count}");
            var allMatches = new HashSet<Block>(initialMatches);
            var toCheck = new Queue<Block>(initialMatches);

            while (toCheck.Count > 0)
            {
                var block = toCheck.Dequeue();
                var blockPos = block.GetComponent<RectTransform>().anchoredPosition;
                var newPos = new Tuple<float, float>(blockPos.x, blockPos.y);

                foreach (var dir in new[] { Vector2.up * _blockGap, Vector2.down * _blockGap, Vector2.left * _blockGap, Vector2.right * _blockGap })
                {
                    var adjacentPos = new Tuple<float, float>(newPos.Item1 + dir.x, newPos.Item2 + dir.y);
                    
                    Debug.Log($"{adjacentPos} 위치 블록 검증");
                    if (_tiles.ContainsKey(adjacentPos) && _tiles[adjacentPos].Type == block.Type && allMatches.Add(_tiles[adjacentPos]))
                    {
                        Debug.Log($"조건 충족, 좌표 {adjacentPos} / 타입 {block.Type} / 삽입 가능 {allMatches.Add(_tiles[adjacentPos])}");
                        toCheck.Enqueue(_tiles[adjacentPos]);
                    }
                }
            }
            
            Debug.Log($"인접한 동일 타입 블록 체크 종료, 현재 {allMatches.Count}");

            return new List<Block>(allMatches);
        }

        /// <summary>
        /// 모든 블록에 대해 매칭을 검사합니다.
        /// </summary>
        /// <param name="tiles">검사할 블록 딕셔너리</param>
        /// <returns>매칭된 블록 목록</returns>
        public List<Block> FindAllMatches(Dictionary<Tuple<float, float>, Block> tiles)
        {
            var matchedBlocks = new List<Block>();

            foreach (var tile in tiles)
            {
                var position = tile.Key;
                if (CheckMatchesForBlock(position, out var matches))
                {
                    matchedBlocks.AddRange(matches);
                }
            }

            return matchedBlocks;
        }
    }
}