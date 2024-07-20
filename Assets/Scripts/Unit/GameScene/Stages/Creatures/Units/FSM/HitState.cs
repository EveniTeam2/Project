using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class HitState : BaseState
    {
        protected readonly HitStateInfo _hitStateInfo;
        private Animator _animator;

        public HitState(BaseStateInfo baseStateInfo, HitStateInfo hitStateInfo, Func<StateType, bool> tryChangeState) : base(baseStateInfo, tryChangeState)
        {
            this._hitStateInfo = hitStateInfo;
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