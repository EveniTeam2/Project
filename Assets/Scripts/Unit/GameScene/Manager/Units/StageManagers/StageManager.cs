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
    public class StageManager : MonoBehaviour, IStage, ISendCommand
    {
        public event Action OnCommandDequeue;
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

        public void Initialize(CharacterData characterData, Vector3 playerSpawnPosition, SceneExtraSetting extraSetting, Camera cam, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            _stageScore = new StageScore();
            ChangeAnimationParameterToHash();
            
            InitializeCharacter(characterData, playerSpawnPosition, blockInfo);
            InitializeMonster(extraSetting, playerSpawnPosition, _stageScore);
            InitializeCamera(cam, extraSetting.cameraSpawnPosition);
            InitializeMap(extraSetting.mapPrefab, playerSpawnPosition);
            
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

        public void RegisterHandleSendCommand(ISendCommand data)
        {
            data.OnSendCommand += OnSendCommand;
        }
        
        private void HandleCommandDequeue()
        {
            OnCommandDequeue?.Invoke();
        }
        
        private void InitializeCharacter(CharacterData characterData, Vector3 playerSpawnPosition, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            // Core.Utils.AddressableLoader.DeployAsset(settings.characterRef, settings.playerPosition, Quaternion.identity, null, (obj) => {
            //     if (obj.TryGetComponent(out _character))
            //         _character.Initialize(settings.characterStat, _backgroundDisplay);
            // });
            
            var character = Instantiate(characterData.CharacterDataSo.creature, playerSpawnPosition, Quaternion.identity);
            
            _zeroPosition = playerSpawnPosition;
            _startTime = Time.time;
            
            if (character.TryGetComponent(out _character))
            {
                _character.Initialize(characterData, playerSpawnPosition.y, _animationParameters, blockInfo);
                OnSendCommand += _character.HandleReceiveCommand;
                _character.OnCommandDequeue += HandleCommandDequeue;
            }
            _character.FsmSystem.RegisterOnDeathState(PlayerIsDead);
        }

        private void InitializeMonster(SceneExtraSetting extraSetting, Vector3 playerSpawnPosition, StageScore stageScore)
        {
            _monsterManager = new MonsterSpawnManager(_character.transform, extraSetting.monsterSpawnData, playerSpawnPosition.y, stageScore, _animationParameters);
        }

        protected virtual void InitializeCamera(Camera cam, Vector3 cameraSpawnPosition)
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
}