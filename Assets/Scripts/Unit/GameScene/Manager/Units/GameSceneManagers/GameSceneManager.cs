using ScriptableObjects.Scripts.Creature.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Manager.Units.StageManagers.Modules;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.CardFactories.Units;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Creatures.Data.CharacterDatas;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems;
using Unit.GameScene.Units.Creatures.Units;
using Unit.GameScene.Units.Panels.Controllers;
using Unit.GameScene.Units.Panels.Modules.StageModules;
using Unit.GameScene.Units.SkillFactories.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkillFactories;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.GameSceneManagers
{
    public class GameSceneManager : MonoBehaviour
    {
        #region Inspector Fields

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

        #region Private Fields

        private RectTransform _comboBlockPanel;
        private RectTransform _matchBlockPanel;
        private RectTransform _cardPanel;
    
        private CharacterData _characterData;
        private ComboBoardController _comboBoardController;
        private MatchBoardController _matchBoardController;
        private CardController _cardController;
    
        private StageScore _stageScore;
        private MonsterSpawner _monsterSpawner;
        private float _startTime;
        private Vector3 _zeroPosition;
    
        private Camera _camera;
        private Canvas _canvas;
        private Character _character;

        private Dictionary<AnimationParameterEnums, int> _animationParameters = new();
        private Dictionary<string, CharacterSkill> _characterSkills;
        private HashSet<Card> _cardInfos = new();
    
        private readonly Dictionary<BlockType, CharacterSkill> _blockInfo = new();

        private event Action<BlockType> OnUpdateCharacterSkillOnBlock;

        #endregion

        #region Properties

        public StageScore StageScore => _stageScore;
        public LinkedList<Monster> Monsters => _monsterSpawner.Monsters;
        public float PlayTime => Time.time - _startTime;
        public float Distance => _character.transform.position.x - _zeroPosition.x;

        #endregion

        #region Unity Lifecycle Methods

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

            StartCoroutine(StageScoreUpdate(_stageScore));
        }

        private void Update()
        {
            _monsterSpawner.Update();
        }

        #endregion

        #region Initialization Methods

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
                _blockInfo.Add((BlockType)i, i == 0 ? _characterData.CharacterSkillSystem.GetDefaultSkill() : null);
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

        private void InstantiateAndInitializeMatchBoard()
        {
            _matchBoardController = Instantiate(defaultSetting.matchBoardControllerPrefab).GetComponent<MatchBoardController>();
            _matchBoardController.Initialize(extraSetting.blockInfos, _matchBlockPanel, _canvas, _characterData, _blockInfo);

            _matchBoardController.RegisterHandleOnIncreaseDragCount(IncreaseDragCount);
            _matchBoardController.RegisterHandleOnSendCommand(_comboBoardController.HandleInstantiateComboBlock);
            _matchBoardController.RegisterHandleOnSendCommand(_character.HandleOnSendCommand);

            RegisterOnUpdateCharacterSkillOnBlock(_matchBoardController.RegisterHandleOnUpdateCharacterSkillOnBlock);
        }

        private void InstantiateAndInitializeStage()
        {
            _stageScore = new StageScore();
            // _monsterSpawner = new MonsterSpawner(_character.transform, extraSetting.monsterSpawnData, extraSetting.playerSpawnPosition.y, _stageScore, _animationParameters);

            InitializeCamera(_camera, extraSetting.cameraSpawnPosition);
            InitializeMap(extraSetting.mapPrefab, extraSetting.playerSpawnPosition);

            _monsterSpawner.Start();
            _zeroPosition = extraSetting.playerSpawnPosition;
            _startTime = Time.time;
        }

        private void InstantiateAndInitializeCard()
        {
            CreateCardData();
            InstantiateCard();
        }

        #endregion

        #region Helper Methods

        private IEnumerator StageScoreUpdate(StageScore score)
        {
            while (true)
            {
                score.SetStageScore(PlayTime, Distance);
                yield return null;
            }
        }

        private void InitializeCamera(Camera cam, Vector3 cameraSpawnPosition)
        {
            cam.GetComponent<CameraController>().Initialize(_character.transform, cameraSpawnPosition);
        }

        private void InitializeMap(GameObject mapPrefab, Vector3 playerSpawnPosition)
        {
            var backgroundController = Instantiate(mapPrefab);
            backgroundController.transform.position = new Vector3(playerSpawnPosition.x, playerSpawnPosition.y - 1, backgroundController.transform.position.z);
            backgroundController.GetComponent<BackgroundController>().Initialize(_character);
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

            _cardController.RegisterHandleOnRegisterCharacterSkill(OnUpdateCharacterSkillOnBlock);
            _character.RegisterOnHandleOnTriggerCard(_cardController.HandleOnTriggerCard);
        }

        private void IncreaseDragCount(int count)
        {
            dragCount = count;
        }

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

        #endregion
    }
}