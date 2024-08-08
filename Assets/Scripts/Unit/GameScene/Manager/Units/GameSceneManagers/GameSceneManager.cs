using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Scripts.Creature.Data;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.CardFactories.Units;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Creatures.Data.CharacterDatas;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems;
using Unit.GameScene.Units.Creatures.Units;
using Unit.GameScene.Units.Panels.Controllers;
using Unit.GameScene.Units.SkillFactories.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkillFactories;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;
using UnityEngine;
using CharacterStatSystem = Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems.CharacterStatSystem;

namespace Unit.GameScene.Manager.Units.GameSceneManagers
{
    /// <summary>
    ///     게임 씬을 관리하며, 보드와 스테이지 초기화, 드래그 횟수 관리, 게임 종료 등을 처리합니다.
    /// </summary>
    public class GameSceneManager : MonoBehaviour
    {
        private event Action<BlockType> OnUpdateCharacterSkillOnBlock;
        
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

        #endregion

        private RectTransform _comboBlockPanel;
        private RectTransform _matchBlockPanel;
        private RectTransform _cardPanel;
        
        private CharacterData _characterData;
        private ComboBoardController _comboBoardController;
        private MatchBoardController _matchBoardController;
        private CardController _cardController;
        
        private StageManager _stageManager;
        
        private Camera _camera;
        private Canvas _canvas;
        private Character _character;

        private Dictionary<AnimationParameterEnums, int> _animationParameters = new ();
        private Dictionary<string, CharacterSkill> _characterSkills;
        private HashSet<Card> _cardInfos = new();
        
        private readonly Dictionary<BlockType, CharacterSkill> _blockInfo = new ();

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

            InstantiateAndInitializeCard();
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
            var characterDataSo = extraSetting.characterDataSos.Cast<CharacterDataSo>().FirstOrDefault(data => data.type == extraSetting.characterType);
            var skillCsvData = CsvParser.ParseCharacterSkillData(defaultSetting.characterSkillCsv);
            _characterSkills = new CharacterSkillFactory(characterDataSo).CreateSkill(skillCsvData);
            var characterCsvData = CsvParser.ParseCharacterStatData(defaultSetting.characterDataCsv);
            
            var skillInfo = new CharacterSkillSystem(extraSetting.characterType, _characterSkills);
            var statInfo = new CharacterStatSystem(extraSetting.characterType, characterCsvData);
            
            _characterData = new CharacterData(characterDataSo, statInfo, skillInfo);
        }

        private void InstantiateCharacter()
        {
            var character = Instantiate(_characterData.CharacterDataSo.creature, extraSetting.playerSpawnPosition, Quaternion.identity);

            if (!character.TryGetComponent(out _character))
            {
                return;
            }

            _character.Initialize(_characterData, extraSetting.playerSpawnPosition.y, defaultSetting.playerHpHandler, defaultSetting.playerExpHandler, defaultSetting.playerLevelHandler, _animationParameters, _blockInfo);
            _character.RegisterHandleOnPlayerDeath(HandleOnPlayerDeath);
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
            
            if (_canvas.renderMode != RenderMode.ScreenSpaceCamera)
            {
                _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            }
            
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                _canvas.worldCamera = _camera;
            }
            
            _matchBlockPanel = defaultSetting.matchBlockSpawnPanel;
            _comboBlockPanel = defaultSetting.comboBlockSpawnPanel;
        }
        
        private void InstantiateAndInitializeComboBoard()
        {
            _comboBoardController = Instantiate(defaultSetting.comboBoardControllerPrefab).GetComponent<ComboBoardController>();
            _comboBoardController.Initialize(extraSetting.blockInfos, _comboBlockPanel, _characterData, _blockInfo);
            
            _character.RegisterHandleOnCommandDequeue(_comboBoardController.HandleDestroyComboBlock);
            
            RegisterOnUpdateCharacterSkillOnBlock(_comboBoardController.RegisterHandleOnUpdateCharacterSkillOnBlock);
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

            RegisterOnUpdateCharacterSkillOnBlock(_matchBoardController.RegisterHandleOnUpdateCharacterSkillOnBlock);
        }

        /// <summary>
        ///     스테이지를 인스턴스화하고 초기화합니다.
        /// </summary>
        private void InstantiateAndInitializeStage()
        {
            _stageManager = Instantiate(defaultSetting.stageManagerPrefab).GetComponent<StageManager>();
            _stageManager.Initialize(_character, extraSetting.playerSpawnPosition, _animationParameters, extraSetting, _camera);
        }

        private void InstantiateAndInitializeCard()
        {
            CreateCardData();
            InstantiateCard();
        }

        private void CreateCardData()
        {
            var statCardData = CsvParser.ParseStatCardData(defaultSetting.cardCsv);
            var skillCardData = _characterSkills;
            _cardInfos = new CardFactory(statCardData, extraSetting.statCardSos, skillCardData, _character).CreateCard();
        }

        private void InstantiateCard()
        {
            _cardController = Instantiate(defaultSetting.cardControllerPrefab).GetComponent<CardController>();
            _cardController.Initialize(defaultSetting.cardPanel, defaultSetting.cardSpawnPanel, _cardInfos, _blockInfo);

            // CardController에 OnUpdateCharacterSkillOnBlock 이벤트 등록
            _cardController.RegisterHandleOnRegisterCharacterSkill(OnUpdateCharacterSkillOnBlock);
            _character.RegisterOnHandleOnTriggerCard(_cardController.HandleOnTriggerCard);
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
        // Enum 타입에 대한 해시값을 미리 계산해서 저장합니다.
        private void ChangeAnimationParameterToHash()
        {
            _animationParameters = new Dictionary<AnimationParameterEnums, int>();

            for (var i = 0; i < Enum.GetValues(typeof(AnimationParameterEnums)).Length; i++)
            {
                var targetEnum = (AnimationParameterEnums)i;
                _animationParameters.Add(targetEnum, Animator.StringToHash($"{targetEnum}"));
                Debug.Log($"{targetEnum} => {Animator.StringToHash($"{targetEnum}")} 파싱");
            }
        }

        private void HandleOnPlayerDeath()
        {
            // TODO : 캐릭터가 죽었을 때, 게임 멈추고 팝업 띄우기
            Debug.Log("넥서스를 파괴하면숴ㅓㅓㅓㅓㅓ 쥐쥐~~~!~!~!~!~!~!~!");
        }

        private void RegisterOnUpdateCharacterSkillOnBlock(Action<BlockType> action)
        {
            OnUpdateCharacterSkillOnBlock += action;
        }
    }
}