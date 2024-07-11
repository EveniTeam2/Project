#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.Character;
using UnityEngine;

namespace TestScripts {
    public class TestSceneManager : MonoBehaviour {
        [SerializeField] BattleStageSetting settings;
        [SerializeField] private BattleStageManager stageManager;
        [SerializeField] private List<CommandToStage> testCommand;
        [SerializeField] private KeyCode testKey;
        private void Start() {
            stageManager.Initialize(settings);
        }

        private void Update() {
            if (Input.GetKeyDown(testKey)) {
                foreach (var command in testCommand) {
                    stageManager.Received(command);
                }
                Debug.Log("Test Command execute");
            }
        }
    }
    [Serializable]
    public class CommandToStage : ICommand<IBattleStageTarget> {
        [SerializeField] NewBlock block;
        [SerializeField] int count;
        [SerializeField] float targetNormalTime;
        void ICommand<IBattleStageTarget>.Execute(IBattleStageTarget target) {
            target.Player.Input(block, count);
        }
        bool ICommand<IBattleStageTarget>.IsExecutable(IBattleStageTarget target) {
            return (target.Player.HFSM.GetCurrentAnimationNormalizedTime() > targetNormalTime);
        }
    }
}
#endif