using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Boards;
using Unit.GameScene.Boards.Blocks;
using Unit.GameScene.Boards.Blocks.Enums;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Manager.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Unit.GameScene.Manager.Units
{
    /// <summary>
    ///     보드 상태를 관리하며, 블록 교환, 매칭 및 보드 갱신을 처리합니다.
    /// </summary>
    public class BoardManager : MonoBehaviour, ISendCommand, IIncreaseDragCount
    {
        public event Action<int> OnIncreaseDragCount;
        public event Action<ICommand<IStage>> OnSendCommand;
        
        [Header("보드 가로 x 세로 사이즈 (단위 : 칸)")] [SerializeField]
        private int width;

        [SerializeField]
        private int height;

        [Header("각각의 로직 사이의 대기 시간 (단위 : Second)")] [SerializeField] [Range(0, 1f)]
        private float logicProgressTime;

        [Header("블록 스왑 완료까지 걸리는 시간 (단위 : Second)")] [SerializeField] [Range(0, 1f)]
        private float moveDuration;

        [Header("블록 낙하 속도 (단위 : Unit / Second)")] [SerializeField] [Range(0, 100f)]
        private float dropDurationPerUnit;

        [Header("블록 낙하 이후 바운스 높이 (단위 : Unit)")] [SerializeField] [Range(0, 1f)]
        private float bounceHeight;

        [Header("블록 낙하 이후 바운스 대기 시간 (단위 : Second)")] [SerializeField] [Range(0, 1f)]
        private float bounceDuration;

        [Header("블록 풀링 관련 설정")] [SerializeField]
        private Block blockPrefab;
        
        [Header("로직 동작 여부")] [SerializeField]
        private bool isLogicUpdating;
        
        // 하나의 클래스는 하나의 기능을 가진다.

        private IBlockGenerator _blockGenerator;
        private IBlockMatcher _blockMatcher;
        private IBlockMover _blockMover;
        private IBlockPool _blockPool;
        
        private RectTransform _blockPanel;
        private List<BlockSo> _blockInfos;
        private List<Tuple<float, float>> _blockPositions;

        private Vector2 _blockSize;
        private RectTransform _blockSpawnPos;

        private Canvas _canvas;

        [Header("명령 대기 리스트")]
        private OrderedDictionary _currentMatchBlock;

        private int _dragCount;
        
        private float _blockGap;
        private float _halfPanelWidth;
        private int _poolSize;
        
        private WaitForSeconds _progressTime;
        private Dictionary<Tuple<float, float>, Block> _tiles;

        #region #### 보드 초기화 ####

        /// <summary>
        ///     보드를 초기화하고, 블록을 생성합니다.
        /// </summary>
        /// <param name="blockSos">생성할 블록 정보</param>
        /// <param name="blockPanel">블록 생성 위치</param>
        /// <param name="canvas">캔버스</param>
        public void Initialize(List<BlockSo> blockSos, RectTransform blockPanel, Canvas canvas)
        {
            InitializeBoard(blockSos, blockPanel, canvas);
            GenerateAllRandomBlocks();
        }

        /// <summary>
        ///     보드를 초기화합니다. 값 설정, 스폰 위치 계산 및 의존성 등록을 수행합니다.
        /// </summary>
        /// <param name="blockSos">생성할 블록 정보</param>
        /// <param name="blockPanel">블록 생성 위치</param>
        /// <param name="canvas">캔버스</param>
        private void InitializeBoard(List<BlockSo> blockSos, RectTransform blockPanel, Canvas canvas)
        {
            InitializeValues(blockSos, blockPanel, canvas);
            CalculateBlockSpawnPositions();
            RegisterDependencies();
        }

        /// <summary>
        ///     보드 값을 초기화합니다.
        /// </summary>
        /// <param name="blockSos">생성할 블록 정보</param>
        /// <param name="blockPanel">블록 생성 위치</param>
        /// <param name="canvas">캔버스</param>
        private void InitializeValues(List<BlockSo> blockSos, RectTransform blockPanel, Canvas canvas)
        {
            _dragCount = 0;
            isLogicUpdating = false;
            _poolSize = width * height;
            _blockInfos = blockSos;
            _blockPanel = blockPanel;
            _canvas = canvas;

            _tiles = new Dictionary<Tuple<float, float>, Block>();
            _currentMatchBlock = new OrderedDictionary();
            _progressTime = new WaitForSeconds(logicProgressTime);
        }

        /// <summary>
        ///     블록의 스폰 위치를 계산합니다.
        /// </summary>
        private void CalculateBlockSpawnPositions()
        {
            // panelWidth 구하기
            var localScale = _blockPanel.localScale;
            var panelWidth = _blockPanel.parent.GetComponent<RectTransform>().localPosition.x * localScale.x * -2;

            // 블록 간격 계산
            _blockGap = (float) Math.Truncate(panelWidth / (width - 1));
            _blockPositions = new List<Tuple<float, float>>();

            _halfPanelWidth = panelWidth / 2;

            // 블록 크기 계산
            _blockSize = new Vector2(_blockGap * localScale.x, _blockGap * localScale.x);

            // 블록 배치 좌표 계산
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var x = -_halfPanelWidth + i * _blockGap;
                    var y = -_halfPanelWidth + j * _blockGap;
                    
                    _blockPositions.Add(new Tuple<float, float>(x, y));
            
                    Debug.Log($"블록 배치 좌표 : {x}, {y}");
                }
            }
        }

        /// <summary>
        ///     보드에 필요한 의존성을 등록합니다.
        /// </summary>
        private void RegisterDependencies()
        {
            _blockPool = new BlockPool(blockPrefab, _blockPanel, _poolSize, true);
            _blockGenerator = new BlockGenerator(_blockInfos, _blockPool, _tiles, _canvas, _blockPanel, _blockSize, _blockPositions, CheckForMatch, _blockGap);
            _blockMatcher = new BlockMatcher(_tiles, _blockGap);
            _blockMover = new BlockMover(moveDuration, 1 / dropDurationPerUnit, bounceHeight, bounceDuration, _progressTime, _blockGap, this);
        }

        #endregion

        #region #### 블록 생성 ####

        /// <summary>
        ///     모든 블록을 제거하고 재생성합니다.
        /// </summary>
        private void GenerateAllRandomBlocks()
        {
            while (true)
            {
                _blockGenerator.GenerateAllRandomBlocks();

                if (IsAnyPossibleMatches()) break;

                RemoveAllBlocks();
            }
        }

        /// <summary>
        ///     새로운 블록을 생성하고 빈 공간으로 이동할 위치를 설정합니다.
        /// </summary>
        /// <param name="spawnHash">스폰된 블록의 위치 해시셋</param>
        /// <param name="blockDic">이동할 블록 딕셔너리</param>
        /// <param name="pos">새 블록의 위치</param>
        private void GenerateBlockAndSetDestination(HashSet<Tuple<float, float>> spawnHash, Dictionary<Tuple<float, float>, Block> blockDic, Tuple<float, float> pos)
        {
            var adjustY = 0f;

            while (true)
            {
                adjustY += _blockGap;
                var dropPosition = new Tuple<float, float>(pos.Item1, _halfPanelWidth + adjustY);

                if (spawnHash.Contains(dropPosition)) continue;

                var newBlockInfo = _blockGenerator.GetRandomValidBlock(_tiles, pos);
                var block = _blockPool.Get();
                block.GetComponent<RectTransform>().anchoredPosition = new Vector3(dropPosition.Item1, dropPosition.Item2, 0);
                block.Initialize(newBlockInfo, CheckForMatch, _canvas);
                _tiles[pos] = block;

                spawnHash.Add(dropPosition);

                blockDic.Add(pos, block);

                SetDestination(pos.Item1, pos.Item2, blockDic);

                break;
            }
        }

        /// <summary>
        ///     블록이 빈 공간으로 이동할 위치를 설정합니다.
        /// </summary>
        /// <param name="x">블록의 x 좌표</param>
        /// <param name="y">블록의 y 좌표</param>
        /// <param name="blockDic">이동할 블록 딕셔너리</param>
        private void SetDestination(float x, float y, Dictionary<Tuple<float, float>, Block> blockDic)
        {
            for (var aboveY = y + _blockGap; aboveY <= _halfPanelWidth; aboveY += _blockGap)
            {
                var abovePos = new Tuple<float, float>(x, aboveY);
                
                if (!_tiles.ContainsKey(abovePos)) continue;

                var block = _tiles[abovePos];
                _tiles.Remove(abovePos);
                _tiles[new Tuple<float, float>(x, y)] = block;
                blockDic.Add(new Tuple<float, float>(x, y), block);
                break;
            }
        }

        #endregion

        #region #### 블록 제거 ####

        /// <summary>
        ///     여러 블록들을 제거합니다.
        /// </summary>
        /// <param name="blocks">제거 대상 블록 목록</param>
        private void RemoveBlocks(List<Block> blocks)
        {
            Debug.Log("블록 제거");

            foreach (var block in blocks)
            {
                Debug.Log($"드래그 카운트 {_dragCount} / 삭제 블록 타입 : {block.Type}");
                RemoveBlock(block);
            }
        }

        /// <summary>
        ///     블록을 제거합니다.
        /// </summary>
        /// <param name="block">제거할 블록</param>
        private void RemoveBlock(Block block)
        {
            var blockPos = block.GetComponent<RectTransform>().anchoredPosition;

            Debug.Log($"{blockPos} 블록 제거 ");

            _tiles.Remove(new Tuple<float, float>(blockPos.x, blockPos.y));
            _blockPool.Release(block);
        }

        /// <summary>
        ///     모든 블록을 제거합니다.
        /// </summary>
        private void RemoveAllBlocks()
        {
            var allBlocks = _tiles.Select(tile => tile.Value).ToList();

            RemoveBlocks(allBlocks);
        }

        #endregion

        #region #### 블록 이동 ####

        /// <summary>
        ///     블록 교환 및 이후의 낙하, 매칭 및 블록 채우기 작업을 처리합니다.
        /// </summary>
        private IEnumerator ProcessBlockSwapAndFall(Block currentBlock, Block targetBlock,
            Tuple<float, float> currentBlockIndex, Tuple<float, float> targetBlockIndex)
        {
            yield return StartCoroutine(_blockMover.SwapBlock(currentBlock, targetBlock, targetBlockIndex,
                currentBlockIndex));

            // 교환 후에 딕셔너리에서 블록 위치 업데이트
            UpdateBlockPositions(currentBlockIndex, targetBlockIndex, currentBlock, targetBlock);

            // 초기 매칭된 블록 제거
            yield return ProcessMatchedBlocks(currentBlockIndex, targetBlockIndex);

            // 빈 공간 채우기
            yield return FillEmptySpaces();
            yield return _progressTime;

            // 새로운 블록 채우기
            yield return FillNewBlocks();
            yield return _progressTime;

            // 추가 매칭된 블록 제거 반복
            yield return ProcessAdditionalMatches();

            // 더 이상 매칭 가능한 블록이 없으면 모든 블록을 재생성
            while (!IsAnyPossibleMatches())
            {
                RemoveAllBlocks();
                _blockGenerator.GenerateAllRandomBlocks();
            }

            isLogicUpdating = false;

            foreach (DictionaryEntry combo in _currentMatchBlock)
            {
                var key = (Tuple<BlockType, int>)combo.Key;
                var value = (int)combo.Value;

                Debug.Log($"Key: ({key.Item1}, {key.Item2}), Value: {value}");

                OnSendCommand?.Invoke(new CommandPacket(key.Item1, value, 0.5f));
            }

            _currentMatchBlock.Clear();
        }

        /// <summary>
        ///     블록 위치를 업데이트합니다.
        /// </summary>
        private void UpdateBlockPositions(Tuple<float, float> currentBlockIndex, Tuple<float, float> targetBlockIndex,
            Block currentBlock, Block targetBlock)
        {
            _tiles[currentBlockIndex] = targetBlock;
            _tiles[targetBlockIndex] = currentBlock;
        }

        /// <summary>
        ///     매칭된 블록들을 처리합니다.
        /// </summary>
        private IEnumerator ProcessMatchedBlocks(Tuple<float, float> currentBlockIndex,
            Tuple<float, float> targetBlockIndex)
        {
            _blockMatcher.CheckMatchesForBlock(targetBlockIndex, out var currentMatchedBlocks);
            _blockMatcher.CheckMatchesForBlock(currentBlockIndex, out var targetMatchedBlocks);

            if (currentMatchedBlocks.Count > 0) CheckBlockCombo(currentMatchedBlocks[0].Type);
            if (targetMatchedBlocks.Count > 0) CheckBlockCombo(targetMatchedBlocks[0].Type);

            var allMatchedBlocks = new HashSet<Block>(_blockMatcher.GetAdjacentMatches(currentMatchedBlocks));
            allMatchedBlocks.UnionWith(_blockMatcher.GetAdjacentMatches(targetMatchedBlocks));

            Debug.Log($"삭제될 블록 수 {allMatchedBlocks.Count}");
            RemoveBlocks(allMatchedBlocks.ToList());
            Debug.Log($"남은 블록 수 {_tiles.Count}");

            yield return null;
        }

        private void CheckBlockCombo(BlockType type)
        {
            var key = new Tuple<BlockType, int>(type, _dragCount);

            if (_currentMatchBlock.Contains(key))
                _currentMatchBlock[key] = (int)_currentMatchBlock[key] + 1;
            else
                _currentMatchBlock.Add(key, 1);
        }

        /// <summary>
        ///     인접한 매칭된 블록들을 그룹으로 나눕니다.
        /// </summary>
        /// <param name="blocks">매칭된 블록 목록</param>
        /// <returns>블록 그룹 목록</returns>
        private List<List<Block>> GroupAdjacentMatches(List<Block> blocks)
        {
            var visited = new HashSet<Block>();
            var groups = new List<List<Block>>();

            foreach (var block in blocks)
            {
                if (visited.Contains(block)) continue;

                var group = new List<Block>();
                var toVisit = new Queue<Block>();
                toVisit.Enqueue(block);

                while (toVisit.Count > 0)
                {
                    var current = toVisit.Dequeue();
                    if (visited.Contains(current)) continue;

                    visited.Add(current);
                    group.Add(current);

                    var neighbors = _blockMatcher.GetAdjacentMatches(new List<Block> { current });
                    foreach (var neighbor in neighbors)
                        if (!visited.Contains(neighbor) && blocks.Contains(neighbor))
                            toVisit.Enqueue(neighbor);
                }

                if (group.Count > 0) groups.Add(group);
            }

            return groups;
        }

        /// <summary>
        ///     추가 매칭된 블록들을 처리합니다.
        /// </summary>
        private IEnumerator ProcessAdditionalMatches()
        {
            while (true)
            {
                var matchedBlocks = _blockMatcher.FindAllMatches(_tiles);
                if (matchedBlocks.Count == 0) break;

                var groupedMatches = GroupAdjacentMatches(matchedBlocks);

                foreach (var group in groupedMatches)
                {
                    // 콤보를 계산합니다.
                    if (group.Count >= 3) CheckBlockCombo(group[0].Type);

                    Debug.Log($"삭제될 블록 그룹 수 {groupedMatches.Count}");
                    RemoveBlocks(group);
                }

                Debug.Log($"남은 블록 수 {_tiles.Count}");

                yield return FillEmptySpaces();
                yield return _progressTime;

                yield return FillNewBlocks();
                yield return _progressTime;
            }
        }

        /// <summary>
        ///     보드의 빈 공간을 블록을 아래로 이동시켜 채웁니다.
        /// </summary>
        private IEnumerator FillEmptySpaces()
        {
            Debug.Log("빈 공간으로 블록 이동");

            var blockDic = new Dictionary<Tuple<float, float>, Block>();

            for (var x = -_halfPanelWidth; x <= _halfPanelWidth; x += _blockGap)
            {
                for (var y = -_halfPanelWidth; y <= _halfPanelWidth; y += _blockGap)
                {
                    var pos = new Tuple<float, float>(x, y);
                    if (_tiles.ContainsKey(pos)) continue;

                    Debug.Log($"{pos} 블록 이동");

                    SetDestination(x, y, blockDic);
                }
            }

            yield return DropBlocks(blockDic.Values);
        }

        /// <summary>
        ///     보드의 빈 공간을 새 블록으로 채웁니다.
        /// </summary>
        private IEnumerator FillNewBlocks()
        {
            Debug.Log("새 블록 생성");

            var spawnHash = new HashSet<Tuple<float, float>>();
            var blockDic = new Dictionary<Tuple<float, float>, Block>();

            for (var x = -_halfPanelWidth; x <= _halfPanelWidth; x += _blockGap)
            for (var y = -_halfPanelWidth; y <= _halfPanelWidth; y += _blockGap)
            {
                var pos = new Tuple<float, float>(x, y);
                if (_tiles.ContainsKey(pos)) continue;

                GenerateBlockAndSetDestination(spawnHash, blockDic, pos);
            }

            yield return DropBlocks(blockDic.Values);
        }

        /// <summary>
        ///     블록을 목표 위치로 떨어뜨리는 코루틴을 실행합니다.
        /// </summary>
        /// <param name="blocks">이동할 블록 목록</param>
        private IEnumerator DropBlocks(IEnumerable<Block> blocks)
        {
            var dropCoroutines = new List<Coroutine>();

            foreach (var block in blocks)
            {
                var targetPos = _tiles.FirstOrDefault(t => t.Value == block).Key;
                dropCoroutines.Add(StartCoroutine(_blockMover.DropBlock(targetPos, block)));
            }

            foreach (var coroutine in dropCoroutines) yield return coroutine;
        }

        #endregion

        #region #### 블록 이동 검증 로직 ####

        /// <summary>
        ///     블록을 드래그하여 새 위치로 이동할 때 매치를 확인합니다.
        /// </summary>
        /// <param name="startPosition">블록의 시작 위치</param>
        /// <param name="direction">드래그 방향</param>
        private void CheckForMatch(Vector3 startPosition, Vector3 direction)
        {
            if (isLogicUpdating) return;
            isLogicUpdating = true;

            var currentBlockIndex = new Tuple<float, float>(startPosition.x, startPosition.y);
            var targetBlockIndex = _blockMatcher.GetTargetIndex(startPosition, direction);

            Debug.Log($"{startPosition} 블록 스왑 검증, 타겟 {targetBlockIndex}");

            if (!_blockMatcher.IsValidPosition(targetBlockIndex))
            {
                Debug.Log("유효하지 않은 위치");
                isLogicUpdating = false;
                return;
            }

            if (CheckSwapForMatch(currentBlockIndex, targetBlockIndex))
            {
                Debug.Log("스왑 시작");

                OnIncreaseDragCount?.Invoke(++_dragCount);

                var currentBlock = _tiles[currentBlockIndex];
                var targetBlock = _tiles[targetBlockIndex];
                StartCoroutine(ProcessBlockSwapAndFall(currentBlock, targetBlock, currentBlockIndex, targetBlockIndex));
            }
            else
            {
                Debug.Log("스왑 실패");
                isLogicUpdating = false;
            }
        }

        /// <summary>
        ///     보드에 드래그하여 매칭할 수 있는 블록이 있는지 확인합니다.
        /// </summary>
        /// <returns>매칭할 수 있는 블록이 있으면 true, 없으면 false</returns>
        private bool IsAnyPossibleMatches()
        {
            var tileKeys = new List<Tuple<float, float>>(_tiles.Keys);

            foreach (var position in tileKeys)
            {
                var directions = new[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

                foreach (var direction in directions)
                {
                    var targetIndex = _blockMatcher.GetTargetIndex(new Vector3(position.Item1, position.Item2, 0),
                        new Vector3(direction.x, direction.y, 0));

                    if (_blockMatcher.IsValidPosition(targetIndex) && CheckSwapForMatch(position, targetIndex))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     블록 위치를 스왑하여 매칭 여부를 확인합니다.
        /// </summary>
        /// <param name="startPos">시작 위치</param>
        /// <param name="targetPos">목표 위치</param>
        /// <returns>매칭 여부</returns>
        private bool CheckSwapForMatch(Tuple<float, float> startPos, Tuple<float, float> targetPos)
        {
            var currentBlock = _tiles[startPos];
            var targetBlock = _tiles[targetPos];

            _tiles[startPos] = targetBlock;
            _tiles[targetPos] = currentBlock;

            var currentBlockNewPositionHasMatches = _blockMatcher.CheckMatchesForBlock(targetPos, out _);
            var targetBlockNewPositionHasMatches = _blockMatcher.CheckMatchesForBlock(startPos, out _);

            _tiles[startPos] = currentBlock;
            _tiles[targetPos] = targetBlock;

            return currentBlockNewPositionHasMatches || targetBlockNewPositionHasMatches;
        }

        #endregion
    }
}