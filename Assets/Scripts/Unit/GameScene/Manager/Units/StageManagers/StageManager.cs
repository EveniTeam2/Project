using System;
using System.Collections;
using System.Collections.Generic;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Stages.Backgrounds;
using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    // 채이환 숙제 : Ref 타입과 Value 타입에 대해서 다시 공부하기!
    
    public class StageScore {
        private float score;
        private float playTime;
        private float distance;

        public float Score { get => score; }
        public float PlayTime { get => playTime; }
        public float Distance { get => distance; }

        public void SetStageScore(float time, float distance) {
            playTime = time;
            this.distance = distance;
        }
    }

    public class StageManager : MonoBehaviour, IStage, ICommandReceiver<IStage>
    {
        public event Action OnPlayerDeath;
        
        public StageScore StageScore { get => stageScore; }
        public Character Character => _character;
        public LinkedList<Monster> Monsters => _monsterManager.Monsters;
        
        public float PlayTime => Time.time - _startTime;
        public float Distance => _character.transform.position.x - _zeroPosition.x;
        
        protected StageScore stageScore;
        protected Character _character;
        protected MonsterSpawnManager _monsterManager;
        protected float _startTime;
        protected Vector3 _zeroPosition;
        protected Queue<ICommand<IStage>> _commands = new();
        private readonly Dictionary<AnimationParameterEnums, int> _animationParameter = new ();

        Coroutine _stageScoreCoroutine;

        public void Initialize(CharacterSetting characterSetting, Vector3 playerSpawnPosition, SceneExtraSetting extraSetting, SceneDefaultSetting defaultSetting, Camera cam)
        {
            stageScore = new StageScore();
            ChangeAnimationParameterToHash(defaultSetting.creatureAnimationParameter);
            InitializeCharacter(characterSetting, playerSpawnPosition);
            InitializeMonster(extraSetting, playerSpawnPosition, stageScore);
            InitializeCamera(cam);
            InitializeCommand();

            _monsterManager.Start();
        }

        private void Update()
        {
            UpdateCommand();

            // TODO : _monsterManager null이라 잠깐 막아놨습니다.
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
            Debug.Log("Attach Clear");
            data.OnSendCommand += ReceiveCommand;
        }
        
        public void ReceiveCommand(ICommand<IStage> command)
        {
            _commands.Enqueue(command);
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
                _character.Initialize(characterSetting, playerSpawnPosition.y, _animationParameter);
            }
            _character.GetServiceProvider().RegisterEvent(ECharacterEventType.Death, PlayerIsDead);
        }

        private void InitializeMonster(SceneExtraSetting extraSetting, Vector3 playerSpawnPosition, StageScore stageScore)
        {
            _monsterManager = new MonsterSpawnManager(_character.transform, extraSetting.monsterSpawnData, playerSpawnPosition.y, stageScore, _animationParameter);
        }

        protected virtual void InitializeCamera(Camera cam)
        {
            cam.GetComponent<CameraController>().Initialize(_character.transform);
        }

        private void InitializeCommand()
        {
            if (_commands == null)
            {
                _commands = new Queue<ICommand<IStage>>();
            }
            else
            {
                _commands.Clear();
            }
        }
        
        
        public void UpdateCommand()
        {
            if (_commands.Count > 0 && _commands.Peek().IsExecutable(this))
            {
                Debug.Log("커맨드 Dequeue");
                _commands.Dequeue().Execute(this);
            }
        }

        private void PlayerIsDead() {
            OnPlayerDeath?.Invoke();
        }
        
        private void ChangeAnimationParameterToHash(AnimationParameterEnums[] animationParameterEnums)
        {
            foreach (var animationParameter in animationParameterEnums)
            {
                _animationParameter.Add(animationParameter, Animator.StringToHash($"{animationParameter}"));
            }
        }
    }

    public enum ECharacterEventType {
        Death,
        Damage,

    }
}