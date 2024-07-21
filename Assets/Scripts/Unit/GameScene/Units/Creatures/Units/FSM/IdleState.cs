using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class IdleState : BaseState
    {
        protected BattleSystem _battleSystem;
        protected AnimatorEventReceiver _animatorEventReceiver;

        protected readonly IdleStateInfo _idleStateInfo;

        public IdleState(BaseStateInfo baseInfo, IdleStateInfo idleStateInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver, BattleSystem battleSystem)
            : base(baseInfo, tryChangeState)
        {
            _idleStateInfo = idleStateInfo;
            _animatorEventReceiver = animatorEventReceiver;
            _battleSystem = battleSystem;
        }

        public override void Enter()
        {
            base.Enter();
            _animatorEventReceiver.SetBool(_baseStateInfo.stateParameter, true, null);
            OnFixedUpdate += CheckTargetAndRun;
        }
        public override void Exit()
        {
            base.Exit();
            _animatorEventReceiver.SetBool(_baseStateInfo.stateParameter, false, null);
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