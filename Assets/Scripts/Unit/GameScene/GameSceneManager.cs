using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Boards;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Stages;
using Unit.GameScene.Stages.Backgrounds;
using Unit.GameScene.Stages.Interfaces;
using UnityEngine;

namespace Unit.GameScene
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        
        [Header("Scene 세팅")]
        [SerializeField] private StageSetting settings;
        
        [Header("BoardManager")]
        [SerializeField] private GameObject boardManagerPrefab;
        private BoardManager _boardManager;

        [Header("StageManager")]
        [SerializeField] private GameObject stageManagerPrefab;
        private StageManager _stageManager;
        
        [Header("맵 정보")]
        public GameObject mapPrefab; // TODO : 이것도 나중에 바꿔줘야 함
        private BackgroundController _backgroundController;

        [Header("블록 스폰 위치")]
        [SerializeField] private RectTransform blockPanel;
        
        [Header("캔버스")]
        [SerializeField] private Canvas canvas;
        
        [Header("타일 정보")]
        public List<BlockSo> blockInfos; // TODO : 이전 씬의 게임 매니저에서 값을 전달해주는 것으로 수정
        
        [Header("Test")]
        [SerializeField] private List<CommandToStagePlayer> testCommand;
        [SerializeField] private KeyCode testKey;

        [Header("드래그 횟수")]
        [SerializeField] private int _dragCount;

        private void Awake()
        {
            InstantiateAndInitializeMap();
            InstantiateAndInitializeBoard();
            InstantiateAndInitializeStage();
        }

        //TODO : 테스트용 스크립트
        private void Update() {
            if (Input.GetKeyDown(testKey)) {
                foreach (var command in testCommand) {
                    _stageManager.Received(command);
                }
                Debug.Log("Test Command execute");
            }
        }
        
        private void InstantiateAndInitializeMap()
        {
            _backgroundController = Instantiate(mapPrefab).GetComponent<BackgroundController>();
            _backgroundController.transform.SetParent(mainCamera.transform);
            _backgroundController.Initialize(mainCamera);
        }

        private void InstantiateAndInitializeBoard()
        {
            _boardManager = Instantiate(boardManagerPrefab).GetComponent<BoardManager>();
            _boardManager.Initialize(blockInfos, blockPanel, canvas);
            
            AttachBoard(_boardManager);
        }
        
        private void InstantiateAndInitializeStage()
        {
            _stageManager = Instantiate(stageManagerPrefab).GetComponent<StageManager>();
            _stageManager.Initialize(settings);
            
            _stageManager.AttachBoard(_boardManager);
        }
        
        public void AttachBoard(IIncreaseDragCount data)
        {
            data.OnIncreaseDragCount += IncreaseDragCount;
        }

        private void IncreaseDragCount(int dragCount)
        {
            _dragCount = dragCount;
        }
    }
}