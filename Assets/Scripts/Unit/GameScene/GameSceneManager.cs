using Unit.GameScene.Boards;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Stages;
using Unit.GameScene.Stages.Backgrounds;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene
{
    public class GameSceneManager : MonoBehaviour
    {
        [Header("Scene 추가 세팅")]
        [SerializeField] private SceneExtraSetting extraSetting;
        
        [Header("Scene 기본 세팅")]
        [SerializeField] private SceneDefaultSetting defaultSetting;
        
        [Header("드래그 횟수")]
        [SerializeField] private int dragCount;
        
        private BoardManager _boardManager;
        private StageManager _stageManager;
        private BackgroundController _backgroundController;

        private void Awake()
        {
            InstantiateAndInitializeMap();
            InstantiateAndInitializeBoard();
            InstantiateAndInitializeStage();
        }

        private void InstantiateAndInitializeMap()
        {
            _backgroundController = Instantiate(defaultSetting.mapPrefab).GetComponent<BackgroundController>();
            _backgroundController.transform.SetParent(defaultSetting.mainCamera.transform);
            _backgroundController.Initialize(defaultSetting.mainCamera);
        }

        private void InstantiateAndInitializeBoard()
        {
            _boardManager = Instantiate(defaultSetting.boardManagerPrefab).GetComponent<BoardManager>();
            _boardManager.Initialize(extraSetting.blockInfos, defaultSetting.blockSpawnPos, defaultSetting.canvas);
            
            AttachBoard(_boardManager);
        }
        
        private void InstantiateAndInitializeStage()
        {
            _stageManager = Instantiate(defaultSetting.stageManagerPrefab).GetComponent<StageManager>();
            _stageManager.Initialize(extraSetting);
            
            _stageManager.AttachBoard(_boardManager);
        }

        private void AttachBoard(IIncreaseDragCount data)
        {
            data.OnIncreaseDragCount += IncreaseDragCount;
        }

        private void IncreaseDragCount(int count)
        {
            dragCount = count;
        }
    }
}