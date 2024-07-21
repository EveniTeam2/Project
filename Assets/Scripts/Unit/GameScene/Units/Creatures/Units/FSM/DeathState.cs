using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class DeathState : BaseState
    {
        //private readonly DeathStateInfo _deathInfo;
        private AnimatorEventReceiver _animatorEventReceiver;

        public DeathState(BaseStateInfo baseStateInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver)
            : base(baseStateInfo, tryChangeState)
        {
            //_deathInfo = deathInfo;
            _animatorEventReceiver = animatorEventReceiver;
        }

        public override void Enter()
        {
            base.Enter();
            _animatorEventReceiver.SetTrigger(_baseStateInfo.stateParameter, null);
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}