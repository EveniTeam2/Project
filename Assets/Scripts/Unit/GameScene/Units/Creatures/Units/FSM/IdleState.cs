using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class IdleState : BaseState
    {
        protected BattleSystem _battleSystem;
        protected Animator _animator;

        protected readonly IdleStateInfo _idleStateInfo;

        public IdleState(BaseStateInfo baseInfo, IdleStateInfo idleStateInfo, Func<StateType, bool> tryChangeState, Animator animator, BattleSystem battleSystem)
            : base(baseInfo, tryChangeState)
        {
            _idleStateInfo = idleStateInfo;
            _animator = animator;
            _battleSystem = battleSystem;
        }

        public override void Enter()
        {
            base.Enter();
            _animator.SetBool(_baseStateInfo.stateParameter, true);
            OnFixedUpdate += CheckTargetAndRun;
        }
        public override void Exit()
        {
            base.Exit();
            _animator.SetBool(_baseStateInfo.stateParameter, false);
        }

        protected virtual void CheckTargetAndRun()
        {
            //if (CanTransitionToOther())
            {
                if (!_battleSystem.CheckCollider(_idleStateInfo.targetLayer, _idleStateInfo.direction, _idleStateInfo.distance, out _))
                {
                    OnFixedUpdate -= CheckTargetAndRun;
                    _tryChangeState.Invoke(StateType.Run);
                }
            }
        }
    }
}