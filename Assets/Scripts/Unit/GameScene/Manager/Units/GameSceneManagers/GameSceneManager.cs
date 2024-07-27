using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Scripts.Creature.Data;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Module;
using Unit.GameScene.Units.Blocks.Units.MatchBlock.Enums;
using Unit.GameScene.Units.BoardPanels.Units.ComboBlockPanels.Units;
using Unit.GameScene.Units.BoardPanels.Units.MatchBlockPanels.Units;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Units.CharacterSkills;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.GameSceneManagers
{
    /// <summary>
    ///     게임 씬을 관리하며, 보드와 스테이지 초기화, 드래그 횟수 관리, 게임 종료 등을 처리합니다.
    /// </summary>
    public class GameSceneManager : MonoBehaviour
    {
        // TODO : 이후에 BoardManager와 StageManager 내부의 CharacterManager와 MonsterManager가 해당 액션을 구독하도록 수정
        private event Action<bool> OnGameOver;

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
        private RectTransform _comboBlockEnter;
        private RectTransform _comboBlockExit;
        private RectTransform _matchBlockPanel;
        
        private CharacterData _characterData;
        
        private ComboBoardController _comboBoardController;
        private MatchBoardController _matchBoardController;
        private StageManager _stageManager;
        private Camera _camera;
        private Canvas _canvas;
        private CanvasController _canvasController;

        private Dictionary<BlockType, CharacterSkill> _blockInfo;

        /// <summary>
        ///     게임 씬 매니저 초기화 메서드입니다. 맵, 보드, 스테이지를 초기화합니다.
        /// </summary>
        private void Awake()
        {
            InitializeCharacterData();
            InitializeBlockData();
            
            InstantiateAndInitializeCamera();
            InstantiateAndInitializeCanvas();
            InstantiateAndInitializeComboBoard();
            InstantiateAndInitializeMatchBoard();
            InstantiateAndInitializeStage();
        }

        // /// <summary>
        // ///     게임 시작 시 타이머를 시작합니다.
        // /// </summary>
        // private void Start()
        // {
        //     StartCoroutine(Timer(extraSetting.limitTime));
        // }
        
        private void InitializeCharacterData()
        {
            var characterDataSo = extraSetting.characterData.Cast<CharacterDataSo>().FirstOrDefault(data => data.classType == extraSetting.characterClassType);

            var skillCsvData = CsvParser.ParseCharacterSkillData(extraSetting.skillTextAsset);
            var skills = new CharacterSkillFactory(characterDataSo).CreateSkill();

            var characterCsvData = CsvParser.ParseCharacterStatData(extraSetting.characterTextAsset);

            var skillInfo = new SkillManager(extraSetting.characterClassType, skills, skillCsvData);
            var statInfo = new StatManager(extraSetting.characterClassType, characterCsvData);
            _characterData = new CharacterData(characterDataSo, statInfo, skillInfo);
        }
        
        private void InitializeBlockData()
        {
            _blockInfo = new Dictionary<BlockType, CharacterSkill>();

            for (var i = 0; i < extraSetting.blockInfos.Count; i++)
            {
                _blockInfo.Add((BlockType)i, i == 0 ? _characterData.SkillManager.GetDefaultSkill() : null);
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
        }

        /// <summary>
        ///     보드를 인스턴스화하고 초기화합니다.
        /// </summary>
        private void InstantiateAndInitializeMatchBoard()
        {
            _matchBoardController = Instantiate(defaultSetting.matchBoardControllerPrefab).GetComponent<MatchBoardController>();
            _matchBoardController.Initialize(extraSetting.blockInfos, _matchBlockPanel, _canvas, _characterData, _blockInfo);
            
            _matchBoardController.OnIncreaseDragCount += IncreaseDragCount;
            _matchBoardController.OnSendCommand += _comboBoardController.HandleInstantiateComboBlock;
        }

        /// <summary>
        ///     스테이지를 인스턴스화하고 초기화합니다.
        /// </summary>
        private void InstantiateAndInitializeStage()
        {
            _stageManager = Instantiate(defaultSetting.stageManagerPrefab).GetComponent<StageManager>();
            _stageManager.Initialize(_characterData, extraSetting.playerSpawnPosition, extraSetting, defaultSetting, _camera, _blockInfo);

            _stageManager.RegisterHandleSendCommand(_matchBoardController);
            _stageManager.OnCommandDequeue += _comboBoardController.HandleDestroyComboBlock;
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
    }
}