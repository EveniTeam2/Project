using System;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.StagePanels.Backgrounds;
using UnityEngine;

namespace Temp.TestScripts {
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
                // TODO : 이환님 CommandPacket 구조체로 변경되어서 막아뒀습니다!! 수정해주셔야해요!!
                if (Input.GetKeyDown(command.key))
                {
                    // command.packet.Execute(this);
                }
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
            cam.Initialize(null, cam.transform.position);
        }

        void InitGame() {
            onInput = StopGame;
            var _characterSetting = new CharacterSetting(extraSetting.characterDefaultSetting, extraSetting.characterExtraSetting);
            Initialize(_characterSetting, defaultSetting.playerSpawnPosition, extraSetting, defaultSetting, defaultSetting.mainCamera);
        }

        protected override void InitializeCamera(Camera cam, Vector3 cameraSpawnPosition) {
            this.cam = cam.GetComponent<CameraController>();
            this.cam.Initialize(_character.transform, cameraSpawnPosition);
        }
    }
}