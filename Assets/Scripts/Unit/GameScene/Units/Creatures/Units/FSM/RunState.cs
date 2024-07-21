using System;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class RunState : BaseState
    {
        protected RunStateInfo _runStateInfo;
        private AnimatorEventReceiver _animatorEventReceiver;
        private MovementSystem _movementSystem;
        private BattleSystem _battleSystem;

        public RunState(BaseStateInfo baseInfo, RunStateInfo runStateInfo, Func<StateType, bool> tryChangeState, BattleSystem battleSystem, MovementSystem movementSystem, AnimatorEventReceiver animatorEventReceiver) : base(baseInfo, tryChangeState)
        {
            _runStateInfo = runStateInfo;
            _animatorEventReceiver = animatorEventReceiver;
            _movementSystem = movementSystem;
            _battleSystem = battleSystem;
        }

        public override void Enter()
        {
            base.Enter();
            _animatorEventReceiver.SetBool(_baseStateInfo.stateParameter, true, null);
            _movementSystem.SetRun(true);
            OnFixedUpdate += CheckTargetAndIdle;
        }
        public override void Exit()
        {
            base.Exit();
            _animatorEventReceiver.SetBool(_baseStateInfo.stateParameter, false, null);
            _movementSystem.SetRun(false);
        }
        private void CheckTargetAndIdle()
        {
            //if (CanTransitionToOther())
            {
                if (_battleSystem.CheckCollider(_runStateInfo.targetLayer, _runStateInfo.direction, _runStateInfo.distance, out _))
                {
                    OnFixedUpdate -= CheckTargetAndIdle;
                    _tryChangeState.Invoke(StateType.Idle);
                }
            }
        }
    }
}