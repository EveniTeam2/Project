using System;
using System.Collections.Generic;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Stages.Backgrounds;
using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    // 채이환 숙제 : Ref 타입과 Value 타입에 대해서 다시 공부하기!
    
    public class StageManager : MonoBehaviour, IStage, ICommandReceiver<IStage>
    {
        public event Action<Creature> OnPlayerDeath;
        
        public Character Character => _character;
        public LinkedList<Monster> Monsters => _monsterManager.Monsters;
        public float PlayTime => Time.time - _startTime;
        public float Distance => _character.transform.position.x - _zeroPosition.x;
        
        private Character _character;
        private MonsterSpawnManager _monsterManager;
        private float _startTime;
        private Vector3 _zeroPosition;
        
        private Queue<ICommand<IStage>> _commands = new();
        
        public void Initialize(CharacterSetting characterSetting, Vector3 playerSpawnPosition, SceneExtraSetting extraSetting, SceneDefaultSetting defaultSetting, Camera cam)
        {
            InitializeCharacter(characterSetting, playerSpawnPosition);
            InitializeMonster(extraSetting, playerSpawnPosition);
            InitializeCamera(cam);
            InitializeCommand();

            _monsterManager.Start();
        }
        
        private void Update()
        {
            UpdateCommand();
            
            //TODO : _monsterManager null이라 잠깐 막아놨습니다.
            // _monsterManager.Update();
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
                _character.Initialize(this, characterSetting, playerSpawnPosition.y);
            }
            (_character.Health as IDamageable).OnDeath += PlayerIsDead;
        }

        private void InitializeMonster(SceneExtraSetting extraSetting, Vector3 playerSpawnPosition)
        {
            _monsterManager = new MonsterSpawnManager(this, extraSetting.monsterSpawnData, playerSpawnPosition.y);
        }

        private void InitializeCamera(Camera cam)
        {
            cam.GetComponent<CameraController>().Initialize(_character.transform);
        }

        private void InitializeCommand()
        {
            if (_commands == null)
                _commands = new Queue<ICommand<IStage>>();
            else
                _commands.Clear();
        }
        
        
        public void UpdateCommand()
        {
            if (_commands.Count > 0 && _commands.Peek().IsExecutable(this))
                _commands.Dequeue().Execute(this);
        }

        private void PlayerIsDead(Creature player) {
            OnPlayerDeath?.Invoke(player);
        }
    }
}