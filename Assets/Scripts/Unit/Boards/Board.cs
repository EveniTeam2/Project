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
    /// <summary>
    /// 보드 상태를 관리하며, 블록 교환, 매칭 및 보드 갱신을 처리합니다.
    /// </summary>
    public class Board : MonoBehaviour
    {
        [Header("각각의 로직 사이의 대기 시간 (단위 : Second)")]
        [SerializeField] [Range(0, 0.5f)] private float logicProgressTime;
        private WaitForSeconds _progressTime;
        
        [Header("블록 스왑 완료까지 걸리는 시간 (단위 : Second)")]
        [SerializeField] [Range(0, 0.5f)] private float moveDuration;
        
        [Header("블록 낙하 속도 (단위 : Unit / Second)")]
        [SerializeField] [Range(0, 0.2f)] private float dropDurationPerUnit;
        
        [Header("블록 낙하 이후 바운스 높이 (단위 : Unit)")]
        [SerializeField] [Range(0, 0.5f)] private float bounceHeight;
        
        [Header("블록 낙하 이후 바운스 대기 시간 (단위 : Second)")]
        [SerializeField] [Range(0, 0.5f)] private float bounceDuration;

        [Header("블록 풀링 관련 설정")]
        [SerializeField] private Block blockPrefab;
        [SerializeField] private int poolSize;

        private int _width;
        private int _height;
        private float _spawnPositionWidth;
        private float _spawnPositionHeight;

        private const float BlockOffset = 0.5f;

        private bool isLogicUpdating;

        private BlockGenerator _blockGenerator;
        private BlockMatcher _blockMatcher;
        private BlockMover _blockMover;
        private BlockPool _blockPool;
        
        [Header("생성된 블록 정보 딕셔너리")]
        [SerializeField] private SerializableDictionary<Tuple<float, float>, Block> tiles;

        private void Awake()
        {
            InitializeBoard();
        }

        /// <summary>
        /// 보드를 초기화합니다. 값 설정, 스폰 위치 계산 및 의존성 등록을 수행합니다.
        /// </summary>
        private void InitializeBoard()
        {
            InitializeValues();
            CalculateBlockSpawnPositions();
            RegisterDependencies();
        }

        /// <summary>
        /// 보드 값을 초기화합니다.
        /// </summary>
        private void InitializeValues()
        {
            isLogicUpdating = false;
            _progressTime = new WaitForSeconds(logicProgressTime);
        }

        /// <summary>
        /// 블록의 스폰 위치를 계산합니다.
        /// </summary>
        private void CalculateBlockSpawnPositions()
        {
            _width = GameManager.Instance.boardWidth;
            _height = GameManager.Instance.boardHeight;
            
            var adjustWidth = _width % 2 == 0 ? BlockOffset : 0;
            var adjustHeight = _height % 2 == 0 ? BlockOffset : 0;
            
            _spawnPositionWidth = _width / 2f - adjustWidth;
            _spawnPositionHeight = _height / 2f - adjustHeight;
        }
        
        /// <summary>
        /// 보드에 필요한 의존성을 등록합니다.
        /// </summary>
        private void RegisterDependencies()
        {
            _blockPool = new BlockPool(blockPrefab, transform, poolSize, true);
            _blockGenerator = new BlockGenerator(_spawnPositionWidth, _spawnPositionHeight, GameManager.Instance.blockInfos, CheckForMatch, _blockPool, out tiles);
            _blockMatcher = new BlockMatcher(tiles);
            _blockMover = new BlockMover(moveDuration, dropDurationPerUnit, bounceHeight, bounceDuration, _progressTime, this);
        }

        /// <summary>
        /// 블록을 드래그하여 새 위치로 이동할 때 매치를 확인합니다.
        /// </summary>
        /// <param name="startPosition">블록의 시작 위치</param>
        /// <param name="direction">드래그 방향</param>
        private void CheckForMatch(Vector3 startPosition, Vector3 direction)
        {
            if (isLogicUpdating) return;
            isLogicUpdating = true;
            
            var currentBlockIndex = new Tuple<float, float>(startPosition.x, startPosition.y);
            var targetBlockIndex = _blockMatcher.GetTargetIndex(startPosition, direction);

            if (!_blockMatcher.IsValidPosition(targetBlockIndex)) return;
            
            var currentBlock = tiles[currentBlockIndex];
            var targetBlock = tiles[targetBlockIndex];

            tiles[currentBlockIndex] = targetBlock;
            tiles[targetBlockIndex] = currentBlock;

            var currentBlockNewPositionHasMatches = _blockMatcher.CheckMatchesForBlock(targetBlockIndex, out var currentMatchedBlocks);
            var targetBlockNewPositionHasMatches = _blockMatcher.CheckMatchesForBlock(currentBlockIndex, out var targetMatchedBlocks);

            if (currentBlockNewPositionHasMatches || targetBlockNewPositionHasMatches)
            {
                StartCoroutine(ProcessBlockSwapAndFall(currentBlock, targetBlock, currentBlockIndex, targetBlockIndex, currentMatchedBlocks, targetMatchedBlocks));
            }
            else
            {
                tiles[currentBlockIndex] = currentBlock;
                tiles[targetBlockIndex] = targetBlock;
            }
        }

        /// <summary>
        /// 블록 교환 및 이후의 낙하, 매칭 및 블록 채우기 작업을 처리합니다.
        /// </summary>
        private IEnumerator ProcessBlockSwapAndFall(Block currentBlock, Block targetBlock, Tuple<float, float> currentBlockIndex, Tuple<float, float> targetBlockIndex, List<Block> currentMatchedBlocks, List<Block> targetMatchedBlocks)
        {
            yield return StartCoroutine(_blockMover.SwapBlock(currentBlock, targetBlock, targetBlockIndex, currentBlockIndex));

            RemoveAllMatchedBlocks(currentMatchedBlocks, targetMatchedBlocks);
            
            yield return FillEmptySpaces();
            yield return _progressTime;

            yield return FillNewBlocks();
            yield return _progressTime;

            while (true)
            {
                var matchedBlocks = _blockMatcher.FindAllMatches(tiles);
                if (matchedBlocks.Count == 0) break;

                matchedBlocks = _blockMatcher.GetAdjacentMatches(matchedBlocks);
                
                RemoveMatchedBlocks(matchedBlocks);

                yield return FillEmptySpaces();
                yield return _progressTime;
                
                yield return FillNewBlocks();
                yield return _progressTime;
            }

            isLogicUpdating = false;
        }

        /// <summary>
        /// 보드의 빈 공간을 블록을 아래로 이동시켜 채웁니다.
        /// </summary>
        private IEnumerator FillEmptySpaces()
        {
            var blockDic = new Dictionary<Tuple<float, float>, Block>();

            for (var x = -_spawnPositionWidth; x <= _spawnPositionWidth; x++)
            {
                for (var y = -_spawnPositionHeight; y <= _spawnPositionHeight; y++)
                {
                    var pos = new Tuple<float, float>(x, y);
                    if (tiles.ContainsKey(pos)) continue;

                    SetDestination(x, y, blockDic);
                }
            }

            yield return DropBlocks(blockDic.Values);
        }

        /// <summary>
        /// 보드의 빈 공간을 새 블록으로 채웁니다.
        /// </summary>
        private IEnumerator FillNewBlocks()
        {
            var spawnHash = new HashSet<Tuple<float, float>>();
            var blockDic = new Dictionary<Tuple<float, float>, Block>();

            for (var x = -_spawnPositionWidth; x <= _spawnPositionWidth; x++)
            {
                for (var y = -_spawnPositionHeight; y <= _spawnPositionHeight; y++)
                {
                    var pos = new Tuple<float, float>(x, y);
                    if (tiles.ContainsKey(pos)) continue;

                    GenerateBlockAndSetDestination(spawnHash, blockDic, pos);
                }
            }

            yield return DropBlocks(blockDic.Values);
        }
        
        /// <summary>
        /// 블록이 빈 공간으로 이동할 위치를 설정합니다.
        /// </summary>
        /// <param name="x">블록의 x 좌표</param>
        /// <param name="y">블록의 y 좌표</param>
        /// <param name="blockDic">이동할 블록 딕셔너리</param>
        private void SetDestination(float x, float y, Dictionary<Tuple<float, float>, Block> blockDic)
        {
            for (var aboveY = y + 1; aboveY <= _spawnPositionHeight; aboveY++)
            {
                var abovePos = new Tuple<float, float>(x, aboveY);
                if (!tiles.ContainsKey(abovePos)) continue;

                var block = tiles[abovePos];
                tiles.Remove(abovePos);
                tiles[new Tuple<float, float>(x, y)] = block;
                blockDic.Add(new Tuple<float, float>(x, y), block);
                break;
            }
        }

        /// <summary>
        /// 새로운 블록을 생성하고 빈 공간으로 이동할 위치를 설정합니다.
        /// </summary>
        /// <param name="spawnHash">스폰된 블록의 위치 해시셋</param>
        /// <param name="blockDic">이동할 블록 딕셔너리</param>
        /// <param name="pos">새 블록의 위치</param>
        private void GenerateBlockAndSetDestination(HashSet<Tuple<float, float>> spawnHash, Dictionary<Tuple<float, float>, Block> blockDic, Tuple<float, float> pos)
        {
            var adjustY = 0;

            while (true)
            {
                adjustY++;
                var dropPosition = new Tuple<float, float>(pos.Item1, _spawnPositionHeight + adjustY);

                if (spawnHash.Contains(dropPosition)) continue;

                var newBlockInfo = _blockGenerator.GetRandomValidBlock(tiles, pos);
                var block = _blockPool.Get();
                block.transform.position = new Vector3(dropPosition.Item1, dropPosition.Item2, 0);
                block.Initialize(newBlockInfo, CheckForMatch);
                tiles[pos] = block;

                spawnHash.Add(dropPosition);

                blockDic.Add(pos, block);

                SetDestination(pos.Item1, pos.Item2, blockDic);

                break;
            }
        }

        /// <summary>
        /// 블록을 목표 위치로 떨어뜨리는 코루틴을 실행합니다.
        /// </summary>
        /// <param name="blocks">이동할 블록 목록</param>
        private IEnumerator DropBlocks(IEnumerable<Block> blocks)
        {
            var dropCoroutines = new List<Coroutine>();

            foreach (var block in blocks)
            {
                var targetPos = tiles.FirstOrDefault(t => t.Value == block).Key;
                dropCoroutines.Add(StartCoroutine(_blockMover.DropBlock(targetPos, block)));
            }

            foreach (var coroutine in dropCoroutines)
            {
                yield return coroutine;
            }
        }

        /// <summary>
        /// 매칭된 모든 블록을 제거합니다.
        /// </summary>
        /// <param name="currentMatchedBlocks">현재 매칭된 블록 목록</param>
        /// <param name="targetMatchedBlocks">목표 위치에서 매칭된 블록 목록</param>
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

        /// <summary>
        /// 매칭된 블록을 제거합니다.
        /// </summary>
        /// <param name="matchedBlocks">매칭된 블록 목록</param>
        private void RemoveMatchedBlocks(List<Block> matchedBlocks)
        {
            foreach (var block in matchedBlocks)
            {
                var blockPos = block.transform.position;
                tiles.Remove(new Tuple<float, float>(blockPos.x, blockPos.y));
                _blockPool.Release(block);
            }
        }
    }
}