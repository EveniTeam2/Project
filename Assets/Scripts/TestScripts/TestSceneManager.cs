#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.Stages;
using Unit.Stages.Creatures.Interfaces;
using Unit.Stages.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace TestScripts {
    public class TestSceneManager : MonoBehaviour {
        [SerializeField] StageSetting settings;
        [SerializeField] private StageManager stageManager;
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
    public class CommandToStage : ICommand<IStageCreature> {
        [FormerlySerializedAs("block")] [SerializeField] BlockSo blockSo;
        [SerializeField] int count;
        [SerializeField] float targetNormalTime;
        void ICommand<IStageCreature>.Execute(IStageCreature creature) {
            creature.Character.Input(blockSo, count);
        }
        bool ICommand<IStageCreature>.IsExecutable(IStageCreature creature) {
            return (creature.Character.HFSM.GetCurrentAnimationNormalizedTime() > targetNormalTime);
        }
    }
}
#endif