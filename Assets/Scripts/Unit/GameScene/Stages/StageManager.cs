using System;
using System.Collections.Generic;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Characters;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Monsters;
using Unit.GameScene.Stages.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Stages {
    public class StageManager : MonoBehaviour, IStageCreature, ICommandReceiver<IStageCreature> {
        public PlayerCreature Character => _character;
        private PlayerCreature _character;
        public LinkedList<MonsterCreature> Monsters => _monsterManager.Monsters;

        private Queue<ICommand<IStageCreature>> _commands = new();
        private MonsterSpawnManager _monsterManager;
        public float PlayTime => Time.time - _startTime;
        private float _startTime;
        public float Distance => _character.transform.position.x - _zeroPosition.x;
        private Vector3 _zeroPosition;

        public event Action<BaseCreature> OnPlayerDeath;
        public void AttachBoard(ISendCommand data) {
            
            Debug.Log("Attach Clear");
            data.OnSendCommand += Received;
        }

        // TODO 인호님! 이거 불러야 시작 가능함
        public void Initialize(SceneExtraSetting settings) {
            InitializeCharacter(settings);

            InitializeMonster(settings);

            InitializeCommand();

        }

        private void InitializeCharacter(SceneExtraSetting settings) {
            // Core.Utils.AddressableLoader.DeployAsset(settings.characterRef, settings.playerPosition, Quaternion.identity, null, (obj) => {
            //     if (obj.TryGetComponent(out _character))
            //         _character.Initialize(settings.characterStat, _backgroundDisplay);
            // });
            var character = Instantiate(settings.characterRef, settings.playerPosition, Quaternion.identity);
            // TODO 인호님 여기가 첫 시작에 대한 기준점입니다.
            _zeroPosition = settings.playerPosition;
            _startTime = Time.time;

            if (character.TryGetComponent(out _character)) {
                _character.Initialize(this, settings.characterStat, settings.groundYPosition, settings.actOnInputs.ToArray());
            }
            (_character.Health as IDamageable).OnDeath += PlayerIsDead;
        }

        private void InitializeMonster(SceneExtraSetting settings) {
            _monsterManager = new MonsterSpawnManager(this, settings.monsterSpawnData, settings.groundYPosition);
            _monsterManager.Start();
        }

        private void InitializeCommand() {
            if (_commands == null)
                _commands = new Queue<ICommand<IStageCreature>>();
            else
                _commands.Clear();
        }

        private void PlayerIsDead(BaseCreature player) {
            OnPlayerDeath?.Invoke(player);
        }

        private void Update() {
            (this as ICommandReceiver<IStageCreature>).UpdateCommand();
            _monsterManager.Update();
        }

        public void Received(ICommand<IStageCreature> command) {
            _commands.Enqueue(command);
        }

        void ICommandReceiver<IStageCreature>.UpdateCommand() {
            if (_commands.Count > 0) {
                if (_commands.Peek().IsExecutable(this))
                    _commands.Dequeue().Execute(this);
            }
        }
    }
}
