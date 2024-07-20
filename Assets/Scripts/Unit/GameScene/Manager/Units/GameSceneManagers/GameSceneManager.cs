using System;
using System.Collections;
using ScriptableObjects.Scripts.Creature.Settings.KnightDefaultSetting;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Module;
using Unit.GameScene.Stages.Backgrounds;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.BoardPanels.Units.MatchBlockPanels;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.GameSceneManagers
{
    /// <summary>
    ///     게임 씬을 관리하며, 보드와 스테이지 초기화, 드래그 횟수 관리, 게임 종료 등을 처리합니다.
    /// </summary>
    public class GameSceneManager : MonoBehaviour
    {
        // TODO : 이후에 BoardManager와 StageManager 내부의 CharacterManager와 MonsterManager가 해당 액션을 구독하도록 수정
        protected event Action<bool> OnGameOver;
        
        [Header("==== Scene 추가 세팅 ====")]
        [SerializeField] protected SceneExtraSetting extraSetting;

        [Header("==== Scene 기본 세팅 ====")]
        [SerializeField] protected SceneDefaultSetting defaultSetting;

        [Header("드래그 횟수")]
        [SerializeField] protected int dragCount;
        
        [Header("게임 오버")]
        [SerializeField] protected bool isGameOver;
        
        [Header("현재 진행 시간")]
        [SerializeField] protected float currentTime;

        protected CharacterSetting _characterSetting;
        protected RectTransform _blockPanel;
        protected MatchBoardController MatchBoardController;
        protected StageManager _stageManager;
        protected Camera _camera;
        protected Canvas _canvas;

        /// <summary>
        ///     게임 씬 매니저 초기화 메서드입니다. 맵, 보드, 스테이지를 초기화합니다.
        /// </summary>
        protected virtual void Awake()
        {
            InstantiateAndInitializeCamera();
            InstantiateAndInitializeCanvas();
            InstantiateAndInitializeCharacterSetting();
            InstantiateAndInitializeBoard();
            InstantiateAndInitializeStage();
        }

        protected void InstantiateAndInitializeCharacterSetting()
        {
            _characterSetting = new CharacterSetting(extraSetting.characterDefaultSetting, extraSetting.characterExtraSetting);
        }

        protected void InstantiateAndInitializeCamera()
        {
            _camera = defaultSetting.mainCamera.GetComponent<Camera>();
        }

        protected void InstantiateAndInitializeCanvas()
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

            _blockPanel = _canvas.GetComponent<CanvasController>().MatchBlockPanel;
        }

        /// <summary>
        ///     게임 시작 시 타이머를 시작합니다.
        /// </summary>
        protected virtual void Start()
        {
            StartCoroutine(Timer(extraSetting.limitTime));
        }

        /// <summary>
        ///     보드를 인스턴스화하고 초기화합니다.
        /// </summary>
        protected void InstantiateAndInitializeBoard()
        {
            MatchBoardController = Instantiate(defaultSetting.matchBoardControllerPrefab).GetComponent<MatchBoardController>();
            MatchBoardController.Initialize(extraSetting.blockInfos, _blockPanel, _canvas);

            AttachBoard(MatchBoardController);
        }

        /// <summary>
        ///     스테이지를 인스턴스화하고 초기화합니다.
        /// </summary>
        protected void InstantiateAndInitializeStage()
        {
            _stageManager = Instantiate(defaultSetting.stageManagerPrefab).GetComponent<StageManager>();
            _stageManager.Initialize(_characterSetting, defaultSetting.playerSpawnPosition, extraSetting, defaultSetting, _camera);

            _stageManager.RegisterEventHandler(MatchBoardController);
        }

        /// <summary>
        ///     드래그 횟수 증가 이벤트를 보드에 연결합니다.
        /// </summary>
        protected void AttachBoard(IIncreaseDragCount data)
        {
            data.OnIncreaseDragCount += IncreaseDragCount;
        }

        /// <summary>
        ///     드래그 횟수를 증가시킵니다.
        /// </summary>
        /// <param name="count">드래그 횟수</param>
        protected void IncreaseDragCount(int count)
        {
            dragCount = count;
        }

        /// <summary>
        ///     제한 시간 타이머 코루틴입니다.
        /// </summary>
        /// <param name="limitTime">제한 시간</param>
        protected IEnumerator Timer(float limitTime)
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