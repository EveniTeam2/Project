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
    public class MonsterTest : StageManager, IStage {
        [SerializeField] SceneExtraSetting extraSetting;
        [SerializeField] SceneDefaultSetting defaultSetting;
        [SerializeField] KeyCode _restart;
        [SerializeField] TestCommand[] testCommand;
        CameraController cam;
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

            Destroy(_character);
            _monsterManager.Clear();
            cam.Initialize(null);
        }

        void InitGame() {
            onInput = StopGame;
            var _characterSetting = new CharacterSetting(extraSetting.characterDefaultSetting, extraSetting.characterExtraSetting);
            Initialize(_characterSetting, defaultSetting.playerSpawnPosition, extraSetting, defaultSetting, defaultSetting.mainCamera);
        }

        protected override void InitializeCamera(Camera cam) {
            this.cam = cam.GetComponent<CameraController>();
            this.cam.Initialize(_character.transform);
        }
    }
}