using System;
using System.Collections;
using System.Collections.Generic;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Module;
using Unit.GameScene.Stages.Backgrounds;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Units.CharacterSkills;
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
        
        [Header("==== Scene 추가 세팅 ====")]
        [SerializeField] private SceneExtraSetting extraSetting;

        [Header("==== Scene 기본 세팅 ====")]
        [SerializeField] private SceneDefaultSetting defaultSetting;

        [Header("드래그 횟수")]
        [SerializeField] private int dragCount;
        
        [Header("게임 오버")]
        [SerializeField] private bool isGameOver;
        
        [Header("현재 진행 시간")]
        [SerializeField] private float currentTime;

        private CharacterSetting _characterSetting;
        private RectTransform _blockPanel;
        private BackgroundController _backgroundController;
        private BoardManager _boardManager;
        private StageManager _stageManager;
        private Camera _camera;
        private Canvas _canvas;

        /// <summary>
        ///     게임 씬 매니저 초기화 메서드입니다. 맵, 보드, 스테이지를 초기화합니다.
        /// </summary>
        private void Awake()
        {
            InstantiateAndInitializeCharacterSetting();
            InstantiateAndInitializeCamera();
            InstantiateAndInitializeCanvas();
            InstantiateAndInitializeMap();
            InstantiateAndInitializeBoard();
            InstantiateAndInitializeStage();
        }

        private void InstantiateAndInitializeCharacterSetting()
        {
            _characterSetting = new CharacterSetting(extraSetting.characterDefaultSetting, extraSetting.characterExtraSetting);
        }

        private void InstantiateAndInitializeCamera()
        {
            _camera = Instantiate(defaultSetting.mainCamera).GetComponent<Camera>();
        }

        private void InstantiateAndInitializeCanvas()
        {
            _canvas = Instantiate(defaultSetting.canvasController.Canvas);

            if (_canvas.renderMode != RenderMode.ScreenSpaceCamera)
            {
                _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            }
            
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                _canvas.worldCamera = _camera;
            }

            _blockPanel = defaultSetting.canvasController.BlockPanel;
        }

        /// <summary>
        ///     게임 시작 시 타이머를 시작합니다.
        /// </summary>
        private void Start()
        {
            StartCoroutine(Timer(extraSetting.limitTime));
        }

        /// <summary>
        ///     맵을 인스턴스화하고 초기화합니다.
        /// </summary>
        private void InstantiateAndInitializeMap()
        {
            _backgroundController = Instantiate(extraSetting.mapPrefab).GetComponent<BackgroundController>();
            _backgroundController.transform.SetParent(_camera.transform);
            _backgroundController.Initialize(_camera);
        }

        /// <summary>
        ///     보드를 인스턴스화하고 초기화합니다.
        /// </summary>
        private void InstantiateAndInitializeBoard()
        {
            _boardManager = Instantiate(defaultSetting.boardManagerPrefab).GetComponent<BoardManager>();
            _boardManager.Initialize(extraSetting.blockInfos, _blockPanel, _canvas);

            AttachBoard(_boardManager);
        }

        /// <summary>
        ///     스테이지를 인스턴스화하고 초기화합니다.
        /// </summary>
        private void InstantiateAndInitializeStage()
        {
            _stageManager = Instantiate(defaultSetting.stageManagerPrefab).GetComponent<StageManager>();
            _stageManager.Initialize(_characterSetting, defaultSetting.playerSpawnPosition, extraSetting, defaultSetting, _camera);

            _stageManager.RegisterEventHandler(_boardManager);
        }

        /// <summary>
        ///     드래그 횟수 증가 이벤트를 보드에 연결합니다.
        /// </summary>
        private void AttachBoard(IIncreaseDragCount data)
        {
            data.OnIncreaseDragCount += IncreaseDragCount;
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