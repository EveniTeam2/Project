using System;
using System.Collections.Generic;
using Unit.Blocks;
using UnityEngine;

namespace Unit.Boards
{
    public class BlockMatcher
    {
        private readonly Dictionary<Tuple<float, float>, Block> _tiles;

        public BlockMatcher(Dictionary<Tuple<float, float>, Block> tiles)
        {
            _tiles = tiles;
        }

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

        public bool IsValidPosition(Tuple<float, float> position)
        {
            return _tiles.ContainsKey(position);
        }

        public bool CheckMatchesForBlock(Tuple<float, float> position, out List<Block> matchedBlocks)
        {
            matchedBlocks = new List<Block>();

            if (CheckDirection(position, Vector2.up, Vector2.down, out var verticalMatches))
            {
                matchedBlocks.AddRange(verticalMatches);
            }

            if (CheckDirection(position, Vector2.left, Vector2.right, out var horizontalMatches))
            {
                matchedBlocks.AddRange(horizontalMatches);
            }

            if (matchedBlocks.Count > 0)
            {
                matchedBlocks.Add(_tiles[position]);
                matchedBlocks = GetAdjacentMatches(matchedBlocks);
                return true;
            }

            return false;
        }

        private bool CheckDirection(Tuple<float, float> start, Vector2 dir1, Vector2 dir2, out List<Block> matches)
        {
            matches = new List<Block>();
            matches.AddRange(CountMatchesInDirection(start, dir1));
            matches.AddRange(CountMatchesInDirection(start, dir2));

            return matches.Count >= 2; // 자신 포함 3개 이상 매칭
        }

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

        private List<Block> GetAdjacentMatches(List<Block> initialMatches)
        {
            var allMatches = new HashSet<Block>(initialMatches);
            var toCheck = new Queue<Block>(initialMatches);

            while (toCheck.Count > 0)
            {
                var block = toCheck.Dequeue();
                var pos = new Tuple<float, float>(block.transform.position.x, block.transform.position.y);

                foreach (var dir in new[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
                {
                    var adjacentPos = new Tuple<float, float>(pos.Item1 + dir.x, pos.Item2 + dir.y);
                    if (_tiles.ContainsKey(adjacentPos) && _tiles[adjacentPos].Type == block.Type && !allMatches.Contains(_tiles[adjacentPos]))
                    {
                        allMatches.Add(_tiles[adjacentPos]);
                        toCheck.Enqueue(_tiles[adjacentPos]);
                    }
                }
            }

            return new List<Block>(allMatches);
        }
    }
}