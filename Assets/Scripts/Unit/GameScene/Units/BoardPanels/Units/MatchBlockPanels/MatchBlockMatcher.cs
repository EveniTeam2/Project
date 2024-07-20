using System;
using System.Collections.Generic;
using Unit.GameScene.Boards.Blocks;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Units.Blocks.Units.MatchBlock;
using UnityEngine;

namespace Unit.GameScene.Boards
{
    /// <summary>
    ///     블록의 매칭을 검사하는 클래스입니다.
    /// </summary>
    public class MatchBlockMatcher : IBlockMatcher
    {
        private readonly float _blockGap;
        private readonly Dictionary<Tuple<float, float>, MatchMatchBlockView> _tiles;

        public MatchBlockMatcher(Dictionary<Tuple<float, float>, MatchMatchBlockView> tiles, float blockGap)
        {
            _tiles = tiles;
            _blockGap = blockGap;
        }

        /// <summary>
        ///     블록이 이동할 목표 위치를 계산합니다.
        /// </summary>
        /// <param name="startPosition">시작 위치</param>
        /// <param name="direction">방향</param>
        /// <returns>목표 위치</returns>
        public Tuple<float, float> GetTargetIndex(Vector2 startPosition, Vector2 direction)
        {
            var targetX = direction.x > 0 ? startPosition.x + _blockGap :
                direction.x < 0 ? startPosition.x - _blockGap : startPosition.x;
            var targetY = direction.y > 0 ? startPosition.y + _blockGap :
                direction.y < 0 ? startPosition.y - _blockGap : startPosition.y;

            return new Tuple<float, float>(targetX, targetY);
        }

        /// <summary>
        ///     주어진 위치가 유효한지 확인합니다.
        /// </summary>
        /// <param name="position">위치</param>
        /// <returns>유효 여부</returns>
        public bool IsValidPosition(Tuple<float, float> position)
        {
            return _tiles.ContainsKey(position);
        }

        /// <summary>
        ///     블록이 주어진 위치로 이동했을 때 매칭을 검사합니다.
        /// </summary>
        /// <param name="position">위치</param>
        /// <param name="matchedBlocks">매칭된 블록 목록</param>
        /// <returns>매칭 여부</returns>
        public bool CheckMatchesForBlock(Tuple<float, float> position, out List<MatchMatchBlockView> matchedBlocks)
        {
            matchedBlocks = new List<MatchMatchBlockView>();

            if (CheckDirection(position, Vector2.up * _blockGap, Vector2.down * _blockGap, out var verticalMatches))
                matchedBlocks.AddRange(verticalMatches);

            if (CheckDirection(position, Vector2.left * _blockGap, Vector2.right * _blockGap,
                    out var horizontalMatches)) matchedBlocks.AddRange(horizontalMatches);

            if (matchedBlocks.Count > 0)
            {
                matchedBlocks.Add(_tiles[position]);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     초기 매칭된 블록들의 인접 블록도 매칭된 블록으로 추가합니다.
        /// </summary>
        /// <param name="initialMatches">초기 매칭된 블록 목록</param>
        /// <returns>인접한 매칭된 블록 목록</returns>
        public List<MatchMatchBlockView> GetAdjacentMatches(List<MatchMatchBlockView> initialMatches)
        {
            var allMatches = new HashSet<MatchMatchBlockView>(initialMatches);
            var toCheck = new Queue<MatchMatchBlockView>(initialMatches);

            while (toCheck.Count > 0)
            {
                var block = toCheck.Dequeue();
                var blockPos = block.GetComponent<RectTransform>().anchoredPosition;
                var newPos = new Tuple<float, float>(blockPos.x, blockPos.y);

                foreach (var neighbor in GetAllNeighbors(block))
                    if (!allMatches.Contains(neighbor) && neighbor.Type == block.Type)
                    {
                        allMatches.Add(neighbor);
                        toCheck.Enqueue(neighbor);
                    }
            }

            return new List<MatchMatchBlockView>(allMatches);
        }

        /// <summary>
        ///     모든 블록에 대해 매칭을 검사합니다.
        /// </summary>
        /// <param name="tiles">블록 딕셔너리</param>
        /// <returns>매칭된 블록 목록</returns>
        public List<MatchMatchBlockView> FindAllMatches(Dictionary<Tuple<float, float>, MatchMatchBlockView> tiles)
        {
            var matchedBlocks = new List<MatchMatchBlockView>();

            foreach (var tile in tiles)
            {
                var position = tile.Key;
                if (CheckMatchesForBlock(position, out var matches)) matchedBlocks.AddRange(matches);
            }

            var allMatchedBlocks = new HashSet<MatchMatchBlockView>(matchedBlocks);
            var toCheck = new Queue<MatchMatchBlockView>(matchedBlocks);

            while (toCheck.Count > 0)
            {
                var block = toCheck.Dequeue();
                var blockPos = block.GetComponent<RectTransform>().anchoredPosition;

                foreach (var neighbor in GetAllNeighbors(block))
                    if (!allMatchedBlocks.Contains(neighbor) && neighbor.Type == block.Type)
                    {
                        allMatchedBlocks.Add(neighbor);
                        toCheck.Enqueue(neighbor);
                    }
            }

            return new List<MatchMatchBlockView>(allMatchedBlocks);
        }

        /// <summary>
        ///     주어진 방향으로 매칭을 검사합니다.
        /// </summary>
        /// <param name="start">시작 위치</param>
        /// <param name="dir1">방향 1</param>
        /// <param name="dir2">방향 2</param>
        /// <param name="matches">매칭된 블록 목록</param>
        /// <returns>매칭 여부</returns>
        private bool CheckDirection(Tuple<float, float> start, Vector2 dir1, Vector2 dir2, out List<MatchMatchBlockView> matches)
        {
            matches = new List<MatchMatchBlockView>();
            matches.AddRange(CountMatchesInDirection(start, dir1));
            matches.AddRange(CountMatchesInDirection(start, dir2));

            return matches.Count >= 2;
        }

        /// <summary>
        ///     주어진 방향으로 몇 개의 블록이 매칭되는지 셉니다.
        /// </summary>
        /// <param name="start">시작 위치</param>
        /// <param name="direction">방향</param>
        /// <returns>매칭된 블록 목록</returns>
        private List<MatchMatchBlockView> CountMatchesInDirection(Tuple<float, float> start, Vector2 direction)
        {
            var matches = new List<MatchMatchBlockView>();
            var x = start.Item1;
            var y = start.Item2;

            while (true)
            {
                x += direction.x;
                y += direction.y;

                var pos = new Tuple<float, float>(x, y);
                if (!_tiles.ContainsKey(pos) || _tiles[pos].Type != _tiles[start].Type) break;

                matches.Add(_tiles[pos]);
            }

            return matches;
        }

        /// <summary>
        ///     주어진 블록의 모든 이웃 블록을 가져옵니다.
        /// </summary>
        /// <param name="matchMatchBlockView">블록</param>
        /// <returns>이웃 블록 목록</returns>
        private List<MatchMatchBlockView> GetAllNeighbors(MatchMatchBlockView matchMatchBlockView)
        {
            var neighbors = new List<MatchMatchBlockView>();
            var directions = new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            var blockPos = matchMatchBlockView.GetComponent<RectTransform>().anchoredPosition;

            foreach (var direction in directions)
            {
                var neighborPos = new Tuple<float, float>(blockPos.x + direction.x * _blockGap,
                    blockPos.y + direction.y * _blockGap);
                if (_tiles.ContainsKey(neighborPos)) neighbors.Add(_tiles[neighborPos]);
            }

            return neighbors;
        }
    }
}