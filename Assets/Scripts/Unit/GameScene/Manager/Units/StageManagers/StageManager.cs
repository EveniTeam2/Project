using System;
using System.Collections.Generic;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Stages.Backgrounds;
using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Characters;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Monsters;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    // 채이환 숙제 : Ref 타입과 Value 타입에 대해서 다시 공부하기!
    
    public class StageManager : MonoBehaviour, IStage, ICommandReceiver<IStage>
    {
        public float PlayTime => Time.time - _startTime;
        public float Distance => _character.transform.position.x - _zeroPosition.x;
        
        public Character Character => _character;
        public LinkedList<Monster> Monsters => _monsterManager.Monsters;

        public event Action<BaseCreature> OnPlayerDeath;

        private Queue<ICommand<IStage>> _commands = new();
        
        private Character _character;
        private MonsterSpawnManager _monsterManager;
        private float _startTime;
        private Vector3 _zeroPosition;
        
        private void Update()
        {
            UpdateCommand();
            //TODO : _monsterManager null이라 잠깐 막아놨습니다.
            // _monsterManager.Update();
        }

        public void Received(ICommand<IStage> command)
        {
            _commands.Enqueue(command);
        }

        public void AttachBoard(ISendCommand data)
        {
            Debug.Log("Attach Clear");
            data.OnSendCommand += Received;
        }

        // TODO 인호님! 이거 불러야 시작 가능함
        public void Initialize(SceneExtraSetting extraSetting, SceneDefaultSetting defaultSetting, Camera cam)
        {
            InitializeCharacter(extraSetting, defaultSetting);
            InitializeMonster(extraSetting, defaultSetting);
            InitializeCamera(cam);
            InitializeCommand();

            _monsterManager.Start();
        }

        private void InitializeCharacter(SceneExtraSetting extraSettings, SceneDefaultSetting defaultSetting)
        {
            // Core.Utils.AddressableLoader.DeployAsset(settings.characterRef, settings.playerPosition, Quaternion.identity, null, (obj) => {
            //     if (obj.TryGetComponent(out _character))
            //         _character.Initialize(settings.characterStat, _backgroundDisplay);
            // });
            
            var character = Instantiate(extraSettings.characterRef, defaultSetting.playerPosition, Quaternion.identity);
            // TODO 인호님 여기가 첫 시작에 대한 기준점입니다.
            _zeroPosition = defaultSetting.playerPosition;
            _startTime = Time.time;

            if (character.TryGetComponent(out _character))
            {
                _character.Initialize(this, extraSettings.characterStat, defaultSetting.playerPosition.y, defaultSetting.actOnInputs.ToArray());   
            }
            (_character.Health as IDamageable).OnDeath += PlayerIsDead;
        }

        private void InitializeMonster(SceneExtraSetting extraSetting, SceneDefaultSetting defaultSetting)
        {
            _monsterManager = new MonsterSpawnManager(this, extraSetting.monsterSpawnData, defaultSetting.playerPosition.y);
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

        private void PlayerIsDead(BaseCreature player) {
            OnPlayerDeath?.Invoke(player);
        }

        public void UpdateCommand() {
            if (_commands.Count > 0) {
                if (_commands.Peek().IsExecutable(this))
                    _commands.Dequeue().Execute(this);
            }
        }
    }
}