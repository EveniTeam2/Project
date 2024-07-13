using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.Boards;
using Unit.Stages;
using Unit.Stages.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class SceneManager : MonoBehaviour
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

        [Header("블록 스폰 위치")]
        [SerializeField] private RectTransform blockPanel;
        
        [Header("캔버스")]
        [SerializeField] private Canvas canvas;
        
        [Header("타일 정보")]
        public List<NewBlock> blockInfos; // TODO : 이전 씬의 게임 매니저에서 값을 전달해주는 것으로 수정
        
        [Header("Test")]
        [SerializeField] private List<CommandToStagePlayer> testCommand;
        [SerializeField] private KeyCode testKey;

        private void Awake()
        {
            // CalculateScreenSize();
            InstantiateAndInitializeBoard();
            InstantiateAndInitializeStage();
        }

        private void CalculateScreenSize()
        {
            // float screenWidth = Screen.width;
            // float screenHeight = Screen.height;
            //
            // Debug.Log(nameof(screenWidth) + $" : {screenWidth}");
            // Debug.Log(nameof(screenHeight) + $" : {screenHeight}");
            //
            // // 화면의 절반 높이 계산
            // var halfScreenHeight = screenHeight / 2f;
            //
            // Debug.Log(nameof(halfScreenHeight) + $" : {halfScreenHeight}");
            //
            // // 카메라를 기준으로 월드 좌표 계산
            // var topBoxPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenWidth / 2f, screenHeight - halfScreenHeight / 2f, 0));
            // var bottomBoxPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenWidth / 2f, halfScreenHeight / 2f, 0));
            //
            // Debug.Log(nameof(bottomBoxPosition) + $" : {bottomBoxPosition}");
            //
            // // 위치 설정
            // topBox.transform.position = new Vector3(topBoxPosition.x, topBoxPosition.y, 0);
            // bottomBox.transform.position = new Vector3(bottomBoxPosition.x, bottomBoxPosition.y, 0);
            //
            // Debug.Log(nameof(bottomBox.transform.position) + $" : {bottomBox.transform.position}");
            //
            // // 크기 설정
            // var boxScale = mainCamera.ScreenToWorldPoint(new Vector3(screenWidth, halfScreenHeight, mainCamera.nearClipPlane)) - mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
            //
            // Debug.Log(nameof(boxScale) + $" : {boxScale}");
            // bottomBox.transform.localScale = new Vector3(boxScale.x, boxScale.y, 1);
            //
            // Debug.Log(nameof(topBox.transform.localScale) + $" : {topBox.transform.localScale}");
            // Debug.Log(nameof(bottomBox.transform.localScale) + $" : {bottomBox.transform.localScale}");

        }

        private void Update() {
            if (Input.GetKeyDown(testKey)) {
                foreach (var command in testCommand) {
                    _stageManager.Received(command);
                }
                Debug.Log("Test Command execute");
            }
        }

        private void InstantiateAndInitializeBoard()
        {
            _boardManager = Instantiate(boardManagerPrefab).GetComponent<BoardManager>();
            _boardManager.Initialize(blockInfos, blockPanel, canvas);
            
            // 인스턴스화된 게임 오브젝트가 활성화되어 있는지 확인
            if (_boardManager.gameObject.activeSelf)
            {
                Debug.Log("인스턴스화된 게임 오브젝트는 활성화 상태입니다.");
            }
            else
            {
                Debug.Log("인스턴스화된 게임 오브젝트는 비활성화 상태입니다.");
            }
        }
        
        private void InstantiateAndInitializeStage()
        {
            _stageManager = Instantiate(stageManagerPrefab).GetComponent<StageManager>();
            _stageManager.Initialize(settings);
            _stageManager.AttachBoard(_boardManager);
        }
    }
    
    [Serializable]
    public class CommandToStagePlayer : ICommand<IStageCreature> {
        [SerializeField] NewBlock block;
        [SerializeField] int count;
        [SerializeField] float targetNormalTime;
        void ICommand<IStageCreature>.Execute(IStageCreature creature) {
            creature.Character.Input(block, count);
        }
        bool ICommand<IStageCreature>.IsExecutable(IStageCreature creature) {
            return (creature.Character.HFSM.GetCurrentAnimationNormalizedTime() > targetNormalTime);
        }
    }
}