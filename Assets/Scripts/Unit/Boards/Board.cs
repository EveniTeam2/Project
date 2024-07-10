using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using Manager;
using Unit.Blocks;
using UnityEngine;

namespace Unit.Boards
{
    public class Board : MonoBehaviour
    {
        [Header("블록 스왑 이동 완료까지 걸리는 시간 (단위 : 초)")]
        [SerializeField] private float moveDuration;
        
        [Header("각 로직 처리 별로 걸리는 시간 (단위 : 초)")]
        [SerializeField] private float logicProgressTime;
        private WaitForSeconds _progressTime;

        [Header("블록 풀링 관련 설정")]
        [SerializeField] private Block blockPrefab;
        [SerializeField] private int poolSize = 100;

        private int _width;
        private int _height;
        private float _spawnPositionWidth;
        private float _spawnPositionHeight;

        public bool IsLogicRunning { get; private set; }
        
        private const float BlockOffset = 0.5f;

        private BlockGenerator _blockGenerator;
        private BlockMatcher _blockMatcher;
        private BlockMover _blockMover;
        private BlockPool _blockPool;

        [Header("생성된 블록 정보 딕셔너리")]
        [SerializeField] private SerializableDictionary<Tuple<float, float>, Block> _tiles;

        private void Awake()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            InitializeValues();
            CalculateBlockSpawnPositions();
            RegisterDependencies();
        }

        private void InitializeValues()
        {
            IsLogicRunning = false;
            _progressTime = new WaitForSeconds(logicProgressTime);
        }

        private void CalculateBlockSpawnPositions()
        {
            _width = GameManager.Instance.boardWidth;
            _height = GameManager.Instance.boardHeight;
            
            var adjustWidth = _width % 2 == 0 ? BlockOffset : 0;
            var adjustHeight = _height % 2 == 0 ? BlockOffset : 0;
            
            _spawnPositionWidth = _width / 2f - adjustWidth;
            _spawnPositionHeight = _height / 2f - adjustHeight;
        }
        
        private void RegisterDependencies()
        {
            _blockPool = new BlockPool(blockPrefab, transform, poolSize, true);
            _blockGenerator = new BlockGenerator(_spawnPositionWidth, _spawnPositionHeight, GameManager.Instance.blockInfos, CheckForMatch, _blockPool, out _tiles);
            _blockMatcher = new BlockMatcher(_tiles);
            _blockMover = new BlockMover(moveDuration);
        }

        private void CheckForMatch(Vector3 startPosition, Vector3 direction)
        {
            var currentBlockIndex = new Tuple<float, float>(startPosition.x, startPosition.y);
            var targetBlockIndex = _blockMatcher.GetTargetIndex(startPosition, direction);

            if (!_blockMatcher.IsValidPosition(targetBlockIndex)) return;
            
            var currentBlock = _tiles[currentBlockIndex];
            var targetBlock = _tiles[targetBlockIndex];

            _tiles[currentBlockIndex] = targetBlock;
            _tiles[targetBlockIndex] = currentBlock;

            var currentBlockNewPositionHasMatches = _blockMatcher.CheckMatchesForBlock(targetBlockIndex, out var currentMatchedBlocks);
            var targetBlockNewPositionHasMatches = _blockMatcher.CheckMatchesForBlock(currentBlockIndex, out var targetMatchedBlocks);

            if (currentBlockNewPositionHasMatches || targetBlockNewPositionHasMatches)
            {
                StartCoroutine(ProcessBlockSwapAndFall(currentBlock, targetBlock, currentBlockIndex, targetBlockIndex, currentMatchedBlocks, targetMatchedBlocks));
            }
            else
            {
                _tiles[currentBlockIndex] = currentBlock;
                _tiles[targetBlockIndex] = targetBlock;
            }
        }
        
        private IEnumerator ProcessBlockSwapAndFall(Block currentBlock, Block targetBlock, Tuple<float, float> currentBlockIndex, Tuple<float, float> targetBlockIndex, List<Block> currentMatchedBlocks, List<Block> targetMatchedBlocks)
        {
            yield return StartCoroutine(_blockMover.SwapBlock(currentBlock, targetBlock, targetBlockIndex, currentBlockIndex));
            yield return _progressTime;

            RemoveAllMatchedBlocks(currentMatchedBlocks, targetMatchedBlocks);
            yield return _progressTime;
            
            yield return FillEmptySpaces();
            yield return _progressTime;

            yield return FillNewBlocks();
            yield return _progressTime;

            while (true)
            {
                var matchedBlocks = _blockMatcher.FindAllMatches(_tiles);
                if (matchedBlocks.Count == 0) break;

                matchedBlocks = _blockMatcher.GetAdjacentMatches(matchedBlocks);
                
                RemoveMatchedBlocks(matchedBlocks);
                yield return _progressTime;

                yield return FillEmptySpaces();
                yield return _progressTime;

                yield return FillNewBlocks();
                yield return _progressTime;
            }

            IsLogicRunning = false;
        }

        private IEnumerator FillEmptySpaces()
        {
            var blockQueue = new Queue<Tuple<Tuple<float, float>, float, Block>>();
            
            for (var x = -_spawnPositionWidth; x <= _spawnPositionWidth; x++)
            {
                for (var y = -_spawnPositionHeight; y <= _spawnPositionHeight; y++)
                {
                    var pos = new Tuple<float, float>(x, y);
                    if (_tiles.ContainsKey(pos)) continue;

                    for (var aboveY = y + 1; aboveY <= _spawnPositionHeight; aboveY++)
                    {
                        var abovePos = new Tuple<float, float>(x, aboveY);
                        if (!_tiles.ContainsKey(abovePos)) continue;

                        var block = _tiles[abovePos];
                        _tiles.Remove(abovePos);
                        _tiles[pos] = block;
                        
                        var adjustDropSpeed = aboveY - y;
                        var targetPosAndBlock = new Tuple<Tuple<float, float>, float, Block>(pos, adjustDropSpeed, block);
                        blockQueue.Enqueue(targetPosAndBlock);
                        
                        break;
                    }
                }
            }

            var dropCoroutines = new List<Coroutine>();
            
            while (blockQueue.Count > 0)
            {
                var block = blockQueue.Dequeue();
                dropCoroutines.Add(StartCoroutine(_blockMover.DropBlock(block.Item1, block.Item2, block.Item3)));
            }

            foreach (var coroutine in dropCoroutines)
            {
                yield return coroutine;
            }
        }

        private IEnumerator FillNewBlocks()
        {
            var spawnHash = new HashSet<Tuple<float, float>>();
            var blockQueue = new Queue<Tuple<Tuple<float, float>, float, Block>>();
            
            for (var x = -_spawnPositionWidth; x <= _spawnPositionWidth; x++)
            {
                for (var y = -_spawnPositionHeight; y <= _spawnPositionHeight; y++)
                {
                    var pos = new Tuple<float, float>(x, y);
                    
                    if (_tiles.ContainsKey(pos)) continue;
                    
                    var adjustY = 0;
                    
                    while (true)
                    {
                        adjustY++;
                        var dropPosition = new Tuple<float, float>(x, _spawnPositionHeight + adjustY);

                        if (spawnHash.Contains(dropPosition)) continue;
                        
                        var newBlockInfo = _blockGenerator.GetRandomValidBlock(_tiles, pos);
                        var block = _blockPool.Get();
                        block.transform.position = new Vector3(dropPosition.Item1, dropPosition.Item2, 0);
                        block.Initialize(newBlockInfo, CheckForMatch);
                        _tiles[pos] = block;

                        spawnHash.Add(dropPosition);

                        var adjustDropSpeed = dropPosition.Item2 - y;
                        var targetPosAndBlock = new Tuple<Tuple<float, float>, float, Block>(pos, adjustDropSpeed, block);
                        
                        blockQueue.Enqueue(targetPosAndBlock);

                        break;
                    }
                }
            }

            var dropCoroutines = new List<Coroutine>();
            
            while (blockQueue.Count > 0)
            {
                var block = blockQueue.Dequeue();
                dropCoroutines.Add(StartCoroutine(_blockMover.DropBlock(block.Item1, block.Item2, block.Item3)));
            }

            foreach (var coroutine in dropCoroutines)
            {
                yield return coroutine;
            }
        }

        private void RemoveAllMatchedBlocks(List<Block> currentMatchedBlocks, List<Block> targetMatchedBlocks)
        {
            var allMatchedBlocks = new HashSet<Block>();

            if (targetMatchedBlocks.Count > 0)
            {
                var targetMatches = _blockMatcher.GetAdjacentMatches(targetMatchedBlocks);
                allMatchedBlocks.UnionWith(targetMatches);
            }

            if (currentMatchedBlocks.Count > 0)
            {
                var currentMatches = _blockMatcher.GetAdjacentMatches(currentMatchedBlocks);
                allMatchedBlocks.UnionWith(currentMatches);
            }

            RemoveMatchedBlocks(allMatchedBlocks.ToList());
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