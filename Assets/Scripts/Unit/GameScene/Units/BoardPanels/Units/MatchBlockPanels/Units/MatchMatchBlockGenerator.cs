using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Boards.Blocks;
using Unit.GameScene.Boards.Blocks.Enums;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Units.Blocks.Units.MatchBlock;
using Unit.GameScene.Units.BoardPanels.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit.GameScene.Boards
{
    /// <summary>
    ///     블록을 생성하는 클래스입니다.
    /// </summary>
    public class MatchMatchBlockGenerator : IMatchBlockGenerator
    {
        private readonly float _blockGap;
        private readonly RectTransform _blockPanel;
        private readonly IBlockPool _blockPool;
        private readonly List<Tuple<float, float>> _blockPositions;
        private readonly Vector2 _blockSize;
        private readonly List<BlockModel> _blockSos;
        private readonly Canvas _canvas;
        private readonly Action<Vector3, Vector3> _matchCheckHandler;
        private readonly Dictionary<Tuple<float, float>, MatchBlockView> _tiles;
        private Dictionary<BlockType, Sprite> _blockIcons;

        /// <summary>
        ///     BlockGenerator 생성자입니다.
        /// </summary>
        /// <param name="blockSos">블록 정보 목록</param>
        /// <param name="blockPool">블록 풀</param>
        /// <param name="tiles">블록 딕셔너리</param>
        /// <param name="canvas">캔버스</param>
        /// <param name="blockPanel">블록 생성 위치</param>
        /// <param name="blockSize">블록 크기</param>
        /// <param name="blockPositions">블록 위치 목록</param>
        /// <param name="checkForMatch">매치 확인 핸들러</param>
        /// <param name="blockGap">블록 간격</param>
        /// <param name="blockIcons"></param>
        public MatchMatchBlockGenerator(
            List<BlockModel> blockSos,
            IBlockPool blockPool,
            Dictionary<Tuple<float, float>, MatchBlockView> tiles,
            Canvas canvas,
            RectTransform blockPanel,
            Vector2 blockSize,
            List<Tuple<float, float>> blockPositions,
            Action<Vector3, Vector3> checkForMatch,
            float blockGap,
            Dictionary<BlockType, Sprite> blockIcons)
        {
            _blockSos = blockSos;
            _blockPool = blockPool;
            _tiles = tiles;
            _canvas = canvas;
            _blockPanel = blockPanel;
            _blockSize = blockSize;
            _blockPositions = blockPositions;
            _blockGap = blockGap;
            _matchCheckHandler = checkForMatch;
            _blockIcons = blockIcons;
        }

        /// <summary>
        ///     모든 블록을 랜덤하게 생성합니다.
        /// </summary>
        public void GenerateAllRandomBlocks()
        {
            _tiles.Clear();

            foreach (var (x, y) in _blockPositions)
            {
                var block = (MatchBlockView) _blockPool.Get();
                var newBlock = GetRandomValidBlock(_tiles, new Tuple<float, float>(x, y));
                var position = new Vector3(x, y, 0);
                var rectTransform = block.GetComponent<RectTransform>();

                rectTransform.SetParent(_blockPanel);
                rectTransform.sizeDelta = _blockSize;
                rectTransform.anchoredPosition = position;

                block.Initialize(newBlock.type, _matchCheckHandler, _canvas, _blockIcons[newBlock.type], newBlock.background);
                _tiles.Add(new Tuple<float, float>(x, y), block);
            }
        }

        /// <summary>
        ///     주어진 위치에 대해 유효한 랜덤 블록을 가져옵니다.
        /// </summary>
        /// <param name="blockDic">블록 딕셔너리</param>
        /// <param name="position">블록의 위치</param>
        /// <returns>유효한 랜덤 블록</returns>
        public BlockModel GetRandomValidBlock(Dictionary<Tuple<float, float>, MatchBlockView> blockDic,
            Tuple<float, float> position)
        {
            List<BlockModel> validBlocks = new();

            foreach (var blockInfo in _blockSos)
                if (IsValidPosition(blockDic, position, blockInfo))
                    validBlocks.Add(blockInfo);

            return validBlocks.Count == 0
                ? _blockSos[Random.Range(0, _blockSos.Count)]
                : validBlocks[Random.Range(0, validBlocks.Count)];
        }

        /// <summary>
        ///     주어진 위치가 유효한지 확인합니다.
        /// </summary>
        /// <param name="blockDic">블록 딕셔너리</param>
        /// <param name="position">위치</param>
        /// <param name="blockTypeInfo">블록 정보</param>
        /// <returns>유효 여부</returns>
        private bool IsValidPosition(Dictionary<Tuple<float, float>, MatchBlockView> blockDic, Tuple<float, float> position,
            BlockModel blockTypeInfo)
        {
            var directions = new[]
            {
                Vector2.up * _blockGap, Vector2.down * _blockGap, Vector2.left * _blockGap, Vector2.right * _blockGap
            };

            foreach (var direction in directions)
                if (CheckDirection(blockDic, position, blockTypeInfo, direction) >= 2)
                    return false;

            return true;
        }

        /// <summary>
        ///     주어진 방향으로 몇 개의 블록이 매칭되는지 확인합니다.
        /// </summary>
        /// <param name="blockDic">블록 딕셔너리</param>
        /// <param name="position">위치</param>
        /// <param name="blockTypeInfo">블록 정보</param>
        /// <param name="direction">방향</param>
        /// <returns>매칭되는 블록 수</returns>
        private int CheckDirection(Dictionary<Tuple<float, float>, MatchBlockView> blockDic, Tuple<float, float> position,
            BlockModel blockTypeInfo, Vector2 direction)
        {
            var matchCount = 0;
            Tuple<float, float> currentPos = new(position.Item1 + direction.x, position.Item2 + direction.y);

            while (blockDic.ContainsKey(currentPos) && blockDic[currentPos].Type == blockTypeInfo.type)
            {
                matchCount++;
                currentPos = new Tuple<float, float>(currentPos.Item1 + direction.x, currentPos.Item2 + direction.y);
            }

            return matchCount;
        }
    }
}