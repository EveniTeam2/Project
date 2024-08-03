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
using Unit.GameScene.Units.Creatures.Units;
using Unit.GameScene.Units.Panels.Modules.StageModules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    public class StageManager : MonoBehaviour, IStage
    {
        public StageScore StageScore { get => _stageScore; }
        public Character Character => _character;
        public LinkedList<Monster> Monsters => _monsterSpawner.Monsters;
        
        public float PlayTime => Time.time - _startTime;
        public float Distance => _character.transform.position.x - _zeroPosition.x;

        private StageScore _stageScore;
        private Character _character;
        private MonsterSpawner _monsterSpawner;
        private float _startTime;
        private Vector3 _zeroPosition;
        private Dictionary<AnimationParameterEnums, int> _animationParameters;

        Coroutine _stageScoreCoroutine;

        public void Initialize(Character character, Vector3 playerSpawnPosition, Dictionary<AnimationParameterEnums, int> animationParameters, SceneExtraSetting extraSetting, Camera cam)
        {
            _stageScore = new StageScore();
            _animationParameters = animationParameters;
            _character = character;
            
            _monsterSpawner = new MonsterSpawner(_character.transform, extraSetting.monsterSpawnData, playerSpawnPosition.y, _stageScore, _animationParameters);
            InitializeCamera(cam, extraSetting.cameraSpawnPosition);
            InitializeMap(extraSetting.mapPrefab, playerSpawnPosition);
            
            StartCoroutine(StageScoreUpdate(_stageScore));
            
            _monsterSpawner.Start();
            
            _zeroPosition = playerSpawnPosition;
            _startTime = Time.time;
        }

        private void Update()
        {
            _monsterSpawner.Update();
        }

        private IEnumerator StageScoreUpdate(StageScore score) {
            while (true) {
                score.SetStageScore(PlayTime, Distance);
                yield return null;
            }
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