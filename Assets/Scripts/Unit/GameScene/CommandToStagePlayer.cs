using System;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Stages.Interfaces;
using UnityEngine;

namespace Unit.GameScene
{
    [Serializable]
    public class CommandToStagePlayer : ICommand<IStageCreature> {
        [SerializeField] BlockSo blockSo;
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