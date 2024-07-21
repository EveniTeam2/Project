using System;
using System.Collections;
using System.Collections.Generic;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Manager.Units.StageManagers.Modules;
using Unit.GameScene.Stages.Backgrounds;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using Unit.GameScene.Units.Creatures.Units.Characters;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    public class StageManager : MonoBehaviour, IStage, ISendCommand
    {
        public event Action<CommandPacket> OnSendCommand;
        public event Action OnPlayerDeath;
        
        public StageScore StageScore { get => _stageScore; }
        public Character Character => _character;
        public LinkedList<Monster> Monsters => _monsterManager.Monsters;
        
        public float PlayTime => Time.time - _startTime;
        public float Distance => _character.transform.position.x - _zeroPosition.x;
        
        protected StageScore _stageScore;
        protected Character _character;
        protected MonsterSpawnManager _monsterManager;
        protected float _startTime;
        protected Vector3 _zeroPosition;
        private Dictionary<AnimationParameterEnums, int> _animationParameters;

        Coroutine _stageScoreCoroutine;

        public void Initialize(CharacterSetting characterSetting, Vector3 playerSpawnPosition, SceneExtraSetting extraSetting, SceneDefaultSetting defaultSetting, Camera cam)
        {
            _stageScore = new StageScore();
            ChangeAnimationParameterToHash();
            
            InitializeCharacter(characterSetting, playerSpawnPosition);
            InitializeMonster(extraSetting, playerSpawnPosition, _stageScore);
            InitializeCamera(cam);
            InitializeMap(extraSetting.mapPrefab);
            
            StartCoroutine(StageScoreUpdate(_stageScore));
            _monsterManager.Start();
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

        public void RegisterEventHandler(ISendCommand data)
        {
            data.OnSendCommand += OnSendCommand;
        }
        
        private void InitializeCharacter(CharacterSetting characterSetting, Vector3 playerSpawnPosition)
        {
            // Core.Utils.AddressableLoader.DeployAsset(settings.characterRef, settings.playerPosition, Quaternion.identity, null, (obj) => {
            //     if (obj.TryGetComponent(out _character))
            //         _character.Initialize(settings.characterStat, _backgroundDisplay);
            // });
            
            var character = Instantiate(characterSetting.Prefab, playerSpawnPosition, Quaternion.identity);
            
            _zeroPosition = playerSpawnPosition;
            _startTime = Time.time;
            
            if (character.TryGetComponent(out _character))
            {
                _character.Initialize(characterSetting, playerSpawnPosition.y, _animationParameters);
                OnSendCommand += _character.HandleReceiveCommand;
            }
            _character.GetServiceProvider().RegisterEvent(ECharacterEventType.Death, PlayerIsDead);
        }

        private void InitializeMonster(SceneExtraSetting extraSetting, Vector3 playerSpawnPosition, StageScore stageScore)
        {
            _monsterManager = new MonsterSpawnManager(_character.transform, extraSetting.monsterSpawnData, playerSpawnPosition.y, stageScore, _animationParameters);
        }

        protected virtual void InitializeCamera(Camera cam)
        {
            cam.GetComponent<CameraController>().Initialize(_character.transform);
        }

        /// <summary>
        ///     맵을 인스턴스화하고 초기화합니다.
        /// </summary>
        /// <param name="mapPrefab"></param>
        private void InitializeMap(GameObject mapPrefab)
        {
            var backgroundController = Instantiate(mapPrefab);
            // _backgroundController.transform.SetParent(_camera.transform);
            backgroundController.GetComponent<BackgroundController>().Initialize(_character);
        }

        private void PlayerIsDead() {
            OnPlayerDeath?.Invoke();
        }
        
        private void ChangeAnimationParameterToHash()
        {
            _animationParameters = new Dictionary<AnimationParameterEnums, int>();

            for (var i = 0; i < Enum.GetValues(typeof(AnimationParameterEnums)).Length; i++)
            {
                var targetEnum = (AnimationParameterEnums) i;
                
                _animationParameters.Add(targetEnum, Animator.StringToHash($"{targetEnum}"));
                Debug.Log($"{targetEnum} => {Animator.StringToHash($"{targetEnum}")} 파싱");
            }
        }
    }

    public enum ECharacterEventType {
        Death,
        Damage,

    }
}