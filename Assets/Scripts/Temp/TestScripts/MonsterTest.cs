using System;
using System.Collections.Generic;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Manager.Units;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Stages.Backgrounds;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace Temp {
    [Serializable]
    public struct TestCommand {
        public KeyCode key;
        public CommandPacket packet;
    }
    public class MonsterTest : MonoBehaviour, IStage {
        [SerializeField] StageScore stageScore;
        [SerializeField] Unit.GameScene.Stages.Creatures.Units.Characters.Character player;
        [SerializeField] SceneExtraSetting extraSetting;
        [SerializeField] SceneDefaultSetting defaultSetting;
        [SerializeField] KeyCode _restart;
        [SerializeField] TestCommand[] testCommand;

        private float _startTime;
        private MonsterSpawnManager _spawnManager;
        private CameraController cam;

        public float PlayTime => Time.time - _startTime;
        public float Distance => player.transform.position.x - defaultSetting.playerSpawnPosition.x;
        public Unit.GameScene.Stages.Creatures.Units.Characters.Character Character => player;
        public LinkedList<Monster> Monsters => _spawnManager.Monsters;

        void Start() {
            onInput = InitGame;
        }

        void Update() {
            if (Input.GetKeyDown(_restart)) {
                StopOrStartGame();
            }
            foreach (var command in testCommand) {
                if (Input.GetKeyDown(command.key))
                    (command.packet as ICommand<IStage>).Execute(this);
            }
        }
        Action onInput;
        private void StopOrStartGame() {
            onInput?.Invoke();
        }

        void StopGame() {
            onInput = InitGame;

            Destroy(player);
            _spawnManager.Clear();
            cam.Initialize(null);
        }

        void InitGame() {
            onInput = StopGame;
            var _characterSetting = new CharacterSetting(extraSetting.characterDefaultSetting, extraSetting.characterExtraSetting);

            InitializeCharacter(_characterSetting, defaultSetting.playerSpawnPosition);
            InitializeMonster(extraSetting, defaultSetting.playerSpawnPosition);
            InitializeCamera(defaultSetting.mainCamera);

            _spawnManager.Start();
        }

        private void InitializeCharacter(CharacterSetting characterSetting, Vector3 playerSpawnPosition) {
            var character = Instantiate(characterSetting.Prefab, playerSpawnPosition, Quaternion.identity);

            _startTime = Time.time;

            if (character.TryGetComponent(out player)) {
                player.Initialize(characterSetting, playerSpawnPosition.y);
            }
            player.GetServiceProvider().RegistEvent(ECharacterEventType.Death, GetPlayerIsDead);
        }

        private void GetPlayerIsDead() {
            Debug.Log("Player Dead.");
        }

        private void InitializeMonster(SceneExtraSetting extraSetting, Vector3 playerSpawnPosition) {
            _spawnManager = new MonsterSpawnManager(player.transform, extraSetting.monsterSpawnData, playerSpawnPosition.y, stageScore);
        }

        private void InitializeCamera(Camera cam) {
            this.cam = cam.GetComponent<CameraController>();
            this.cam.Initialize(player.transform);
        }
    }
}