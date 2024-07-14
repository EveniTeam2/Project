using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.Boards;
using Unit.Stages;
using Unit.Stages.Backgrounds;
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
        [FormerlySerializedAs("block")] [SerializeField] BlockSo blockSo;
        [SerializeField] int count;
        [SerializeField] float targetNormalTime;
        void ICommand<IStageCreature>.Execute(IStageCreature creature) {
            creature.Character.Input(blockSo, count);
        }
        bool ICommand<IStageCreature>.IsExecutable(IStageCreature creature) {
            return (creature.Character.HFSM.GetCurrentAnimationNormalizedTime() > targetNormalTime);
        }
    }
}