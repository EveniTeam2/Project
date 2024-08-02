using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.UI;
using ScriptableObjects.Scripts.Creature.Data;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Module;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules.Datas;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules.Systems;
using Unit.GameScene.Units.Panels.BoardPanels.Units.ComboBlockPanels.Units;
using Unit.GameScene.Units.Panels.BoardPanels.Units.MatchBlockPanels.Units;
using Unit.GameScene.Units.SkillFactories.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkillFactories;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;
using UnityEngine;
using CharacterStatSystem = Unit.GameScene.Units.Creatures.Units.Characters.Modules.Systems.CharacterStatSystem;

namespace Unit.GameScene.Manager.Units.GameSceneManagers
{
    /// <summary>
    ///     게임 씬을 관리하며, 보드와 스테이지 초기화, 드래그 횟수 관리, 게임 종료 등을 처리합니다.
    /// </summary>
    public class GameSceneManager : MonoBehaviour
    {
        #region Inspector

        [Header("==== Scene 추가 세팅 ===="), SerializeField]
        private SceneExtraSetting extraSetting;

        [Header("==== Scene 기본 세팅 ===="), SerializeField]
        private SceneDefaultSetting defaultSetting;

        [Header("드래그 횟수"), SerializeField]
        private int dragCount;
        
        [Header("게임 오버"), SerializeField]
        private bool isGameOver;
        
        [Header("현재 진행 시간"), SerializeField]
        private float currentTime;

        [Header("카드 정보"), SerializeField]
        private CardGachaPairDatas cardGachaPairDatas;
        [SerializeField]
        private CardGachaPairDatas defaultCardGachaPairDatas;
        [SerializeField]
        private UICardManager uiCardManagerPrefab;

        #endregion

        private RectTransform _comboBlockPanel;
        private RectTransform _comboBlockEnter;
        private RectTransform _comboBlockExit;
        private RectTransform _matchBlockPanel;
        
        private CharacterData _characterData;
        private ComboBoardController _comboBoardController;
        private MatchBoardController _matchBoardController;
        private StageManager _stageManager;
        private CardManager _cardManager;
        private Camera _camera;
        private Canvas _canvas;
        private Character _character;
        private CanvasController _canvasController;

        private Dictionary<AnimationParameterEnums, int> _animationParameters = new ();
        private Dictionary<BlockType, CharacterSkill> _blockInfo = new ();

        public CardManager CardManager => _cardManager;

        /// <summary>
        ///     게임 씬 매니저 초기화 메서드입니다. 맵, 보드, 스테이지를 초기화합니다.
        /// </summary>
        private void Awake()
        {
            ChangeAnimationParameterToHash();
            
            InitializeAndInstantiateCharacter();
            InitializeBlockData();
            
            InstantiateAndInitializeCamera();
            InstantiateAndInitializeCanvas();
            InstantiateAndInitializeComboBoard();
            InstantiateAndInitializeMatchBoard();
            InstantiateAndInitializeStage();

            InstantiateAndInitializeCard(uiCardManagerPrefab, _stageManager);
        }

        // /// <summary>
        // ///     게임 시작 시 타이머를 시작합니다.
        // /// </summary>
        // private void Start()
        // {
        //     StartCoroutine(Timer(extraSetting.limitTime));
        // }
        
        private void InitializeAndInstantiateCharacter()
        {
            CreateCharacterData();
            InstantiateCharacter();
        }

        private void CreateCharacterData()
        {
            var characterDataSo = extraSetting.characterData.Cast<CharacterDataSo>().FirstOrDefault(data => data.classType == extraSetting.characterClassType);
            var skillCsvData = CsvParser.ParseCharacterSkillData(extraSetting.skillTextAsset);
            var skills = new CharacterSkillFactory(characterDataSo).CreateSkill(skillCsvData);
            var characterCsvData = CsvParser.ParseCharacterStatData(extraSetting.characterTextAsset);
            
            var skillInfo = new CharacterSkillSystem(extraSetting.characterClassType, skills);
            var statInfo = new CharacterStatSystem(extraSetting.characterClassType, characterCsvData);
            
            _characterData = new CharacterData(characterDataSo, statInfo, skillInfo);
        }

        private void InstantiateCharacter()
        {
            var character = Instantiate(_characterData.CharacterDataSo.creature, extraSetting.playerSpawnPosition, Quaternion.identity);
            
            if (character.TryGetComponent(out _character))
            {
                _character.Initialize(_characterData, extraSetting.playerSpawnPosition.y, defaultSetting.playerHpPanel, defaultSetting.playerExpPanel, defaultSetting.playerLevelPanel, _animationParameters, _blockInfo);
                _character.RegisterHandleOnPlayerDeath(HandleOnPlayerDeath);
            }
        }

        private void InitializeBlockData()
        {
            for (var i = 0; i < extraSetting.blockInfos.Count; i++)
            {
                _blockInfo.Add((BlockType)i, i == 0 ? _characterData.SkillSystem.GetDefaultSkill() : null);
            }
        }

        private void InstantiateAndInitializeCamera()
        {
            _camera = defaultSetting.mainCamera.GetComponent<Camera>();
        }

        private void InstantiateAndInitializeCanvas()
        {
            _canvas = defaultSetting.canvas.GetComponent<Canvas>();
            _canvasController = _canvas.GetComponent<CanvasController>();
            if (_canvas.renderMode != RenderMode.ScreenSpaceCamera)
            {
                _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            }
            
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                _canvas.worldCamera = _camera;
            }
            
            _matchBlockPanel = _canvasController.MatchBlockPanel;
            _comboBlockPanel = _canvasController.ComboBlockPanel;
            _comboBlockEnter = _canvasController.ComboBlockEnter;
            _comboBlockExit = _canvasController.ComboBlockExit;
        }
        
        private void InstantiateAndInitializeComboBoard()
        {
            _comboBoardController = Instantiate(defaultSetting.comboBoardControllerPrefab).GetComponent<ComboBoardController>();
            _comboBoardController.Initialize(extraSetting.blockInfos, _comboBlockPanel, _comboBlockEnter, _comboBlockExit, _characterData, _blockInfo);
            
            _character.RegisterHandleOnCommandDequeue(_comboBoardController.HandleDestroyComboBlock);
        }

        /// <summary>
        ///     보드를 인스턴스화하고 초기화합니다.
        /// </summary>
        private void InstantiateAndInitializeMatchBoard()
        {
            _matchBoardController = Instantiate(defaultSetting.matchBoardControllerPrefab).GetComponent<MatchBoardController>();
            _matchBoardController.Initialize(extraSetting.blockInfos, _matchBlockPanel, _canvas, _characterData, _blockInfo);

            _matchBoardController.RegisterHandleOnIncreaseDragCount(IncreaseDragCount);
            _matchBoardController.RegisterHandleOnSendCommand(_comboBoardController.HandleInstantiateComboBlock);
            _matchBoardController.RegisterHandleOnSendCommand(_character.HandleOnSendCommand);
        }

        /// <summary>
        ///     스테이지를 인스턴스화하고 초기화합니다.
        /// </summary>
        private void InstantiateAndInitializeStage()
        {
            _stageManager = Instantiate(defaultSetting.stageManagerPrefab).GetComponent<StageManager>();
            _stageManager.Initialize(_character, extraSetting.playerSpawnPosition, _animationParameters, extraSetting, _camera);
        }

        private void InstantiateAndInitializeCard(UICardManager prefab, StageManager stage)
        {
            var ui = Instantiate(prefab);
            ui.gameObject.SetActive(false);
            ui.InitUI();
            _cardManager = new CardManager(ui, stage, defaultCardGachaPairDatas.GetCardGachaPairs(), cardGachaPairDatas.GetCardGachaPairs());
            stage.Character.OnPlayerLevelUp += DrawCard;
        }

        private void DrawCard()
        {
            var cards = _cardManager.GetCards(3);
        }

        /// <summary>
        ///     드래그 횟수를 증가시킵니다.
        /// </summary>
        /// <param name="count">드래그 횟수</param>
        private void IncreaseDragCount(int count)
        {
            dragCount = count;
        }

        /// <summary>
        ///     제한 시간 타이머 코루틴입니다.
        /// </summary>
        /// <param name="limitTime">제한 시간</param>
        private IEnumerator Timer(float limitTime)
        {
            isGameOver = false;

            currentTime = 0f;

            while (currentTime < limitTime)
            {
                currentTime += Time.deltaTime;

                yield return null;
            }

            isGameOver = true;
        }
        
        private void ChangeAnimationParameterToHash()
        {
            _animationParameters = new Dictionary<AnimationParameterEnums, int>();

            for (var i = 0; i < Enum.GetValues(typeof(AnimationParameterEnums)).Length; i++)
            {
                var targetEnum = (AnimationParameterEnums) i;
                
                _animationParameters.Add(targetEnum, Animator.StringToHash($"{targetEnum}"));
                Debug.Log($"{targetEnum} => {Animator.StringToHash($"{targetEnum}")} 파싱");
            }
        }

        private void HandleOnPlayerDeath()
        {
            Debug.Log("넥서스를 파괴하면숴ㅓㅓㅓㅓㅓ 쥐쥐~~~!~!~!~!~!~!~!");
        }
    }
}