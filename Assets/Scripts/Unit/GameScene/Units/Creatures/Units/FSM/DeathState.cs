using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class DeathState : BaseState
    {
        //private readonly DeathStateInfo _deathInfo;
        private Animator _animator;

        public DeathState(BaseStateInfo baseStateInfo, Func<StateType, bool> tryChangeState, Animator animator)
            : base(baseStateInfo, tryChangeState)
        {
            //_deathInfo = deathInfo;
            _animator = animator;
        }

        public override void Enter()
        {
            base.Enter();
            _animator.SetTrigger(_baseStateInfo.stateParameter);
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}