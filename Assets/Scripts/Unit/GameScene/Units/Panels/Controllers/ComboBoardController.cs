using ScriptableObjects.Scripts.Blocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unit.GameScene.Module;
using Unit.GameScene.Units.Blocks.Abstract;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Blocks.Modules;
using Unit.GameScene.Units.Blocks.UI;
using Unit.GameScene.Units.Creatures.Data.CharacterDatas;
using Unit.GameScene.Units.Panels.Interfaces;
using Unit.GameScene.Units.Panels.Modules.BoardModules;
using Unit.GameScene.Units.SkillFactories.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;
using UnityEngine;

namespace Unit.GameScene.Units.Panels.Controllers
{
    public class ComboBoardController : MonoBehaviour
    {
        [Header("보드 Pooling 사이즈 (단위 : 칸)"), SerializeField]
        private int poolSize;
        
        [Header("블록 사이 간격 (단위 : Unit)"), SerializeField]
        private int blockGap;
        
        [Header("블록 이동 속도 (단위 : Unit / Second)"), SerializeField, Range(0, 100f)]
        private float dropDurationPerUnit;
        
        [Header("매치 블록 풀링 관련 설정"), SerializeField]
        private BlockView comboBlockViewPrefab;
        private IBlockPool _blockPool;
        
        private List<BlockModel> _blockInfos;
        private RectTransform _comboBlockPanel;
        private float _comboBlockEnterPosX;
        private float _comboBlockExitPosX;
        private List<float> _blockPositions;
        private Vector2 _blockSize;
        private Dictionary<float, ComboBlockView> _blockViews;
        private Dictionary<BlockType, CharacterSkill> _blockInfo;
        private bool _isProcessing;
        private CharacterData _characterData;
        private Dictionary<string, SkillData> _skillData;
        
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly Queue<Func<Task>> _actions = new();

        public void Initialize(List<BlockModel> blockModels, RectTransform blockPanel, CharacterData characterData, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            StartCoroutine(ComboBoardControllerInitializer(blockModels, blockPanel, characterData, blockInfo));
        }

        private IEnumerator ComboBoardControllerInitializer(List<BlockModel> blockModels, RectTransform blockPanel, CharacterData characterData, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            while (blockPanel.rect.width == 0)
            {
                yield return new WaitForEndOfFrame();
            }
            
            InitializeValues(blockModels, blockPanel, characterData, blockInfo);
            CalculateBlockSpawnPositions();
        }

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

        private void InitializeValues(List<BlockModel> blockInfos, RectTransform comboBlockPanel, CharacterData characterData, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            _blockInfos = blockInfos;
            _comboBlockPanel = comboBlockPanel;
            
            _characterData = characterData;
            
            _blockViews = new Dictionary<float, ComboBlockView>();
            _blockPool = new BlockPool(comboBlockViewPrefab, _comboBlockPanel, poolSize, true);
            
            _blockInfo = blockInfo;
        }
        
        private void CalculateBlockSpawnPositions()
        {
            var rect = _comboBlockPanel.rect;
            var localScaleY = _comboBlockPanel.localScale.y;
            var panelHeight = rect.height;
            
            _comboBlockEnterPosX = rect.max.x + 100;
            _comboBlockExitPosX = rect.min.x - 100;
            
            _blockSize = new Vector2(rect.height * localScaleY, rect.height * localScaleY);
            _blockPositions = new List<float>();
            
            var x = rect.xMin + _blockSize.x / 2;

            for (var i = 0; i < poolSize; i++)
            {
                _blockPositions.Add(x);
                x += _blockSize.x + blockGap;
                Debug.Log($"콤보 패널 블록 좌표 {x} {0}");
            }
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
                
                var spawnPosition = new Vector3(_comboBlockEnterPosX, 0, 0);
                var rectTransform = block.GetComponent<RectTransform>();
                rectTransform.SetParent(_comboBlockPanel);
                rectTransform.sizeDelta = _blockSize;
                
                rectTransform.anchoredPosition = spawnPosition;
                        
                block.Initialize(blockModel.type, comboCount, _blockInfo[blockModel.type], blockModel.background);

                foreach (var blockPosition in _blockPositions.Where(blockPosition => _blockViews.TryAdd(blockPosition, block)))
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
                if (_blockViews.Count > 0)
                {
                    var firstPosition = _blockPositions.First();
                    if (_blockViews.Remove(firstPosition, out var block))
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
            var exitPosition = new Vector3(_comboBlockExitPosX, 0, 0);
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
                
                if (_blockViews.Remove(currentPosition, out var nextBlock))
                {
                    _blockViews.Add(previousPosition, nextBlock);
                    await MoveBlock(nextBlock, previousPosition);
                }
            }
        }
        
        public void RegisterHandleOnUpdateCharacterSkillOnBlock(BlockType type)
        {
            foreach (KeyValuePair<float, ComboBlockView> blockView in _blockViews.Where(blockView => blockView.Value.Type == type))
            {
                blockView.Value.UpdateIcon(_blockInfo[type]);
            }
        }
    }
}