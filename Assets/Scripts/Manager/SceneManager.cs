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
        [SerializeField] private GameObject topBox;
        [SerializeField] private GameObject bottomBox;
        
        [Header("Scene 세팅")]
        [SerializeField] private StageSetting settings;
        
        [Header("BoardManager")]
        [SerializeField] private BoardManager boardManager;
        
        [Header("StageManager")]
        [SerializeField] private StageManager stageManager;
        
        [Header("타일 정보")]
        public List<NewBlock> blockInfos; // TODO : 이전 씬의 게임 매니저에서 값을 전달해주는 것으로 수정
        
        [Header("Test")]
        [SerializeField] private List<CommandToStage> testCommand;
        [SerializeField] private KeyCode testKey;

        private void Awake()
        {
            CalculateScreenSize();
            // InstantiateAndInitializeBoard();
            // InstantiateAndInitializeStage();
        }

        private void CalculateScreenSize()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            
            Debug.Log(nameof(screenWidth) + $" : {screenWidth}");
            Debug.Log(nameof(screenHeight) + $" : {screenHeight}");
        
            // 화면의 절반 높이 계산
            var halfScreenHeight = screenHeight / 2f;
            
            Debug.Log(nameof(halfScreenHeight) + $" : {halfScreenHeight}");

            // 카메라를 기준으로 월드 좌표 계산
            var topBoxPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenWidth / 2f, screenHeight - halfScreenHeight / 2f, 0));
            var bottomBoxPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenWidth / 2f, halfScreenHeight / 2f, 0));

            Debug.Log(nameof(topBoxPosition) + $" : {topBoxPosition}");
            Debug.Log(nameof(bottomBoxPosition) + $" : {bottomBoxPosition}");
            
            // 위치 설정
            topBox.transform.position = new Vector3(topBoxPosition.x, topBoxPosition.y, 0);
            bottomBox.transform.position = new Vector3(bottomBoxPosition.x, bottomBoxPosition.y, 0);
            
            Debug.Log(nameof(topBox.transform.position) + $" : {topBox.transform.position}");
            Debug.Log(nameof(bottomBox.transform.position) + $" : {bottomBox.transform.position}");

            // 크기 설정
            var boxScale = mainCamera.ScreenToWorldPoint(new Vector3(screenWidth, halfScreenHeight, mainCamera.nearClipPlane)) - mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
            
            Debug.Log(nameof(boxScale) + $" : {boxScale}");
            topBox.transform.localScale = new Vector3(boxScale.x, boxScale.y, 1);
            bottomBox.transform.localScale = new Vector3(boxScale.x, boxScale.y, 1);
            
            Debug.Log(nameof(topBox.transform.localScale) + $" : {topBox.transform.localScale}");
            Debug.Log(nameof(bottomBox.transform.localScale) + $" : {bottomBox.transform.localScale}");

        }

        private void Update() {
            if (Input.GetKeyDown(testKey)) {
                foreach (var command in testCommand) {
                    stageManager.Received(command);
                }
                Debug.Log("Test Command execute");
            }
        }

        private void InstantiateAndInitializeBoard()
        {
            Instantiate(boardManager);
            boardManager.Initialize(blockInfos);
        }
        
        private void InstantiateAndInitializeStage()
        {
            Instantiate(stageManager);
            stageManager.Initialize(settings);
            stageManager.AttachBoard(boardManager);
        }
    }
    
    [Serializable]
    public class CommandToStage : ICommand<IStageCreature> {
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