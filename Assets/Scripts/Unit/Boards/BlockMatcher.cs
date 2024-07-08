using System;
using System.Collections.Generic;
using Unit.Blocks;
using UnityEngine;

namespace Unit.Boards
{
    public class BlockMatcher
    {
        private readonly Dictionary<Tuple<float, float>, GameObject> _tiles;
        private readonly Tuple<float, float> _adjustValue = new(1.0f, 1.0f);

        public BlockMatcher(Dictionary<Tuple<float, float>, GameObject> tiles)
        {
            _tiles = tiles;
        }

        public bool IsValidPosition(Tuple<float, float> position)
        {
            return _tiles.ContainsKey(position);
        }

        public Tuple<float, float> GetTargetIndex(Vector3 startPosition, Vector3 direction)
        {
            var targetX = direction.x switch
            {
                > 0 => startPosition.x + _adjustValue.Item1,
                < 0 => startPosition.x - _adjustValue.Item1,
                _ => startPosition.x
            };

            var targetY = direction.y switch
            {
                > 0 => startPosition.y + _adjustValue.Item2,
                < 0 => startPosition.y - _adjustValue.Item2,
                _ => startPosition.y
            };

            return new Tuple<float, float>(targetX, targetY);
        }

        public bool CheckDirection(Tuple<float, float> start, Vector2 dir1, Vector2 dir2)
        {
            var matchCount = 1; // 자신 포함

            matchCount += CountMatchesInDirection(start, dir1);
            matchCount += CountMatchesInDirection(start, dir2);

            return matchCount >= 3;
        }

        private int CountMatchesInDirection(Tuple<float, float> start, Vector2 direction)
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