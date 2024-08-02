using System;
using System.Collections;
using System.Collections.Generic;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Manager.Units.StageManagers.Modules;
using Unit.GameScene.Module;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.Commands;
using Unit.GameScene.Units.Creatures.Units.Characters;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules.Datas;
using Unit.GameScene.Units.Creatures.Units.Monsters;
using Unit.GameScene.Units.Panels.BoardPanels.Units.MatchBlockPanels.Interfaces;
using Unit.GameScene.Units.Panels.StagePanels.Backgrounds;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    public class StageManager : MonoBehaviour, IStage
    {
        public StageScore StageScore { get => _stageScore; }
        public Character Character => _character;
        public LinkedList<Monster> Monsters => _monsterManager.Monsters;
        
        public float PlayTime => Time.time - _startTime;
        public float Distance => _character.transform.position.x - _zeroPosition.x;

        private StageScore _stageScore;
        private Character _character;
        private MonsterSpawner _monsterManager;
        private float _startTime;
        private Vector3 _zeroPosition;
        private Dictionary<AnimationParameterEnums, int> _animationParameters;

        Coroutine _stageScoreCoroutine;

        public void Initialize(Character character, Vector3 playerSpawnPosition, Dictionary<AnimationParameterEnums, int> animationParameters, SceneExtraSetting extraSetting, Camera cam)
        {
            _stageScore = new StageScore();
            _animationParameters = animationParameters;
            _character = character;
            
            InitializeMonster(extraSetting, playerSpawnPosition, _stageScore);
            InitializeCamera(cam, extraSetting.cameraSpawnPosition);
            InitializeMap(extraSetting.mapPrefab, playerSpawnPosition);
            
            StartCoroutine(StageScoreUpdate(_stageScore));
            
            _monsterManager.Start();
            
            _zeroPosition = playerSpawnPosition;
            _startTime = Time.time;
        }

        private void Update()
        {
            _monsterManager.Update();
        }

        private IEnumerator StageScoreUpdate(StageScore score) {
            while (true) {
                score.SetStageScore(PlayTime, Distance);
                yield return null;
            }
        }
        
        private void InitializeMonster(SceneExtraSetting extraSetting, Vector3 playerSpawnPosition, StageScore stageScore)
        {
            _monsterManager = new MonsterSpawner(_character.transform, extraSetting.monsterSpawnData, playerSpawnPosition.y, stageScore, _animationParameters);
        }

        protected void InitializeCamera(Camera cam, Vector3 cameraSpawnPosition)
        {
            cam.GetComponent<CameraController>().Initialize(_character.transform, cameraSpawnPosition);
        }

        /// <summary>
        ///     맵을 인스턴스화하고 초기화합니다.
        /// </summary>
        /// <param name="mapPrefab"></param>
        /// <param name="playerSpawnPosition"></param>
        private void InitializeMap(GameObject mapPrefab, Vector3 playerSpawnPosition)
        {
            var backgroundController = Instantiate(mapPrefab);
            backgroundController.transform.position = new Vector3(playerSpawnPosition.x, playerSpawnPosition.y - 1, backgroundController.transform.position.z);
            backgroundController.GetComponent<BackgroundController>().Initialize(_character);
        }
    }
}