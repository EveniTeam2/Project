using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Units.Blocks.Abstract;
using Unit.GameScene.Units.Blocks.Units.ComboBlock;
using Unit.GameScene.Units.Blocks.Units.MatchBlock.Enums;
using Unit.GameScene.Units.BoardPanels.Interfaces;
using Unit.GameScene.Units.BoardPanels.Modules;
using Unit.GameScene.Units.BoardPanels.Units.ComboBlockPanels.Interfaces;
using Unit.GameScene.Units.BoardPanels.Units.MatchBlockPanels;
using Unit.GameScene.Units.BoardPanels.Units.MatchBlockPanels.Interfaces;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Units.BoardPanels.Units.ComboBlockPanels.Units
{
    public class ComboBoardController : MonoBehaviour
    {
        [Header("보드 Pooling 사이즈 (단위 : 칸)")]
        [SerializeField] private int poolSize;
        
        [Header("블록 사이 간격 (단위 : Unit)")]
        [SerializeField] private int blockGap;
        
        [Header("블록 이동 속도 (단위 : Unit / Second)")] [SerializeField] [Range(0, 100f)]
        private float dropDurationPerUnit;
        
        [Header("매치 블록 풀링 관련 설정")] [SerializeField]
        private BlockView comboBlockViewPrefab;
        
        private IComboBlockMover _blockMover;
            
        private IBlockPool _blockPool;
        
        private List<BlockModel> _blockInfos;
        private RectTransform _comboBlockPanel;
        private RectTransform _comboBlockEnter;
        private RectTransform _comboBlockExit;
        private List<float> _blockPositions;
        private Vector2 _blockSize;
        private Dictionary<float, ComboBlockView> _blocks;
        private Dictionary<BlockType, Sprite> _blockIcons;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly Queue<Func<Task>> _actions = new();
        private bool _isProcessing;

        private void Update()
        {
            if (_actions.Count > 0 && !_isProcessing)
            {
                var action = _actions.Dequeue();
                _isProcessing = true;
                action().ContinueWith(t => _isProcessing = false);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                HandleInstantiateComboBlock(new CommandPacket(BlockType.Skill1, 10));
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                HandleDestroyComboBlock();
            }
        }

        public void Initialize(List<BlockModel> blockInfos, RectTransform comboBlockPanel, RectTransform comboBlockEnter, RectTransform comboBlockExit, List<string> characterSkillPresets, Dictionary<string, Sprite> characterSkillIcons)
        {
            InitializeComboBoard(blockInfos, comboBlockPanel, comboBlockEnter, comboBlockExit, characterSkillPresets, characterSkillIcons);
        }

        private void InitializeComboBoard(List<BlockModel> blockInfos, RectTransform comboBlockPanel, RectTransform comboBlockEnter, RectTransform comboBlockExit, List<string> characterSkillPresets, Dictionary<string, Sprite> characterSkillIcons)
        {
            InitializeValues(blockInfos, comboBlockPanel, comboBlockEnter, comboBlockExit, characterSkillPresets, characterSkillIcons);
            CalculateBlockSpawnPositions();
            RegisterDependencies();
        }

        private void InitializeValues(List<BlockModel> blockInfos, RectTransform comboBlockPanel, RectTransform comboBlockEnter, RectTransform comboBlockExit, List<string> characterSkillPresets, Dictionary<string, Sprite> characterSkillIcons)
        {
            _blockInfos = blockInfos;
            _comboBlockPanel = comboBlockPanel;
            _comboBlockEnter = comboBlockEnter;
            _comboBlockExit = comboBlockExit;
            
            _blocks = new Dictionary<float, ComboBlockView>();
            _blockIcons = new Dictionary<BlockType, Sprite>();
            
            for (var i = 0; i < characterSkillPresets.Count; i++)
            {
                _blockIcons.Add((BlockType) i, characterSkillIcons[characterSkillPresets[i]]);
            }
        }
        
        private void CalculateBlockSpawnPositions()
        {
            var rect = _comboBlockPanel.rect;
            var localScaleY = _comboBlockPanel.localScale.y;
            var panelHeight = rect.height;
            
            _blockSize = new Vector2(panelHeight * localScaleY, panelHeight * localScaleY);
            _blockPositions = new List<float>();
            
            var x = rect.xMin + _blockSize.x / 2;

            for (var i = 0; i < poolSize; i++)
            {
                _blockPositions.Add(x);
                x += _blockSize.x + blockGap;
                Debug.Log($"콤보 패널 블록 좌표 {x} {0}");
            }
        }

        private void RegisterDependencies()
        {
            _blockPool = new BlockPool(comboBlockViewPrefab, _comboBlockPanel, poolSize, true);
            _blockMover = new ComboBlockMover();
        }

        public void HandleInstantiateComboBlock(CommandPacket commandPacket)
        {
            _actions.Enqueue(() => InstantiateComboBlocks(commandPacket.BlockType, commandPacket.ComboCount));
        }

        public void HandleDestroyComboBlock()
        {
            _actions.Enqueue(DestroyComboBlocks);
        }

        private async Task InstantiateComboBlocks(BlockType type, int comboCount)
        {
            await _semaphore.WaitAsync();
            try
            {
                var block = (ComboBlockView)_blockPool.Get();

                var blockModel = ScriptableObject.CreateInstance<BlockModel>();
                
                foreach (var blockInfo in _blockInfos.Where(blockInfo => blockInfo.type == type))
                {
                    blockModel = blockInfo;
                }

                var positionX = _comboBlockEnter.localPosition.x;
                var spawnPosition = new Vector3(positionX, 0, 0);
                var rectTransform = block.GetComponent<RectTransform>();
                rectTransform.SetParent(_comboBlockPanel);
                rectTransform.sizeDelta = _blockSize;
                
                rectTransform.anchoredPosition = spawnPosition;
                        
                block.Initialize(blockModel.type, comboCount, _blockIcons[blockModel.type], blockModel.background);

                foreach (var blockPosition in _blockPositions.Where(blockPosition => _blocks.TryAdd(blockPosition, block)))
                {
                    await MoveBlock(block, blockPosition);
                    break;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task DestroyComboBlocks()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_blocks.Count > 0)
                {
                    var firstPosition = _blockPositions.First();
                    if (_blocks.Remove(firstPosition, out var block))
                    {
                        await RemoveBlock(block);
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task MoveBlock(Component block, float destination)
        {
            var duration = dropDurationPerUnit * 100;
            var currentBlockStartPos = block.GetComponent<RectTransform>().anchoredPosition;
            block.GetComponent<RectTransform>().anchoredPosition = currentBlockStartPos;
            
            var targetPosition = new Vector3(destination, 0, 0);
            var distance = Vector3.Distance(currentBlockStartPos, targetPosition);
            var elapsedTime = 0f;

            Debug.Log($"시작점 {currentBlockStartPos.x} 도착점 {targetPosition}");
                
            while (elapsedTime < distance / duration)
            {
                block.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(currentBlockStartPos, targetPosition, elapsedTime / (distance / duration));
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            block.GetComponent<RectTransform>().anchoredPosition = targetPosition;
        }

        private async Task RemoveBlock(BlockView block)
        {
            var duration = dropDurationPerUnit * 100;
            var currentBlockStartPos = block.GetComponent<RectTransform>().anchoredPosition;
            var exitPosition = new Vector3(_comboBlockExit.localPosition.x, 0, 0);
            var distance = Vector3.Distance(currentBlockStartPos, exitPosition);
            var elapsedTime = 0f;

            while (elapsedTime < distance / duration)
            {
                block.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(currentBlockStartPos, exitPosition, elapsedTime / (distance / duration));
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }

            block.GetComponent<RectTransform>().anchoredPosition = exitPosition;
            _blockPool.Release(block);

            for (var i = 1; i < _blockPositions.Count; i++)
            {
                var currentPosition = _blockPositions[i];
                var previousPosition = _blockPositions[i - 1];
                
                if (_blocks.Remove(currentPosition, out var nextBlock))
                {
                    _blocks.Add(previousPosition, nextBlock);
                    await MoveBlock(nextBlock, previousPosition);
                }
            }
        }
    }
}