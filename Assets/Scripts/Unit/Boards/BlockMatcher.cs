using System;
using System.Collections.Generic;
using Unit.Blocks;
using UnityEngine;

namespace Unit.Boards
{
    /// <summary>
    /// 블록의 매칭을 검사하는 클래스입니다.
    /// </summary>
    public class BlockMatcher
    {
        private readonly Dictionary<Tuple<float, float>, Block> _tiles;

        public BlockMatcher(Dictionary<Tuple<float, float>, Block> tiles)
        {
            _tiles = tiles;
        }

        /// <summary>
        /// 블록이 이동할 목표 위치를 계산합니다.
        /// </summary>
        /// <param name="startPosition">블록의 시작 위치</param>
        /// <param name="direction">블록이 이동할 방향</param>
        /// <returns>블록이 이동할 목표 위치</returns>
        public Tuple<float, float> GetTargetIndex(Vector3 startPosition, Vector3 direction)
        {
            var targetX = direction.x switch
            {
                > 0 => startPosition.x + 1,
                < 0 => startPosition.x - 1,
                _ => startPosition.x
            };

            var targetY = direction.y switch
            {
                > 0 => startPosition.y + 1,
                < 0 => startPosition.y - 1,
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

            // 세로 방향으로 매칭 확인
            if (CheckDirection(position, Vector2.up, Vector2.down, out var verticalMatches))
            {
                matchedBlocks.AddRange(verticalMatches);
            }

            // 가로 방향으로 매칭 확인
            if (CheckDirection(position, Vector2.left, Vector2.right, out var horizontalMatches))
            {
                matchedBlocks.AddRange(horizontalMatches);
            }

            // 매칭된 블록이 있을 경우
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

            // 자신 포함 3개 이상 매칭될 경우
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
            var allMatches = new HashSet<Block>(initialMatches);
            var toCheck = new Queue<Block>(initialMatches);

            while (toCheck.Count > 0)
            {
                var block = toCheck.Dequeue();

                var blockPos = block.transform.position;
                var newPos = new Tuple<float, float>(blockPos.x, blockPos.y);

                foreach (var dir in new[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
                {
                    var adjacentPos = new Tuple<float, float>(newPos.Item1 + dir.x, newPos.Item2 + dir.y);
                    
                    if (_tiles.ContainsKey(adjacentPos) && _tiles[adjacentPos].Type == block.Type && allMatches.Add(_tiles[adjacentPos]))
                    {
                        toCheck.Enqueue(_tiles[adjacentPos]);
                    }
                }
            }

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