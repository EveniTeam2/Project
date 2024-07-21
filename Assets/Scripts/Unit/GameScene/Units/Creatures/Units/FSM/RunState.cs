using System;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class RunState : BaseState
    {
        protected RunStateInfo _runStateInfo;
        private Animator _animator;
        private MovementSystem _movementSystem;
        private BattleSystem _battleSystem;

        public RunState(BaseStateInfo baseInfo, RunStateInfo runStateInfo, Func<StateType, bool> tryChangeState, BattleSystem battleSystem, MovementSystem movementSystem, Animator animator) : base(baseInfo, tryChangeState)
        {
            _runStateInfo = runStateInfo;
            _animator = animator;
            _movementSystem = movementSystem;
            _battleSystem = battleSystem;
        }

        public override void Enter()
        {
            base.Enter();
            _animator.SetBool(_baseStateInfo.stateParameter, true);
            _movementSystem.SetRun(true);
            OnFixedUpdate += CheckTargetAndIdle;
        }
        public override void Exit()
        {
            base.Exit();
            _animator.SetBool(_baseStateInfo.stateParameter, false);
            _movementSystem.SetRun(false);
        }
        private void CheckTargetAndIdle()
        {
            if (_battleSystem.CheckCollider(_runStateInfo.targetLayer, _runStateInfo.direction, _runStateInfo.distance, out _))
            {
                OnFixedUpdate -= CheckTargetAndIdle;
                _tryChangeState.Invoke(StateType.Idle);
            }
        }
    }

    public class MonsterRunState : MonsterBaseState
    {
        protected MonsterRunStateInfo _monsterRunStateInfo;
        private Animator _animator;
        private MovementSystem _movementSystem;
        private BattleSystem _battleSystem;

        public MonsterRunState(MonsterBaseStateInfo monsterBaseInfo,
                               MonsterRunStateInfo monsterRunStateInfo,
                               Func<StateType, bool> tryChangeState,
                               BattleSystem battleSystem,
                               MovementSystem movementSystem,
                               Animator animator,
                               MonsterEventPublisher monsterEventPublisher) : base(monsterBaseInfo, tryChangeState, monsterEventPublisher)
        {
            _monsterRunStateInfo = monsterRunStateInfo;
            _animator = animator;
            _movementSystem = movementSystem;
            _battleSystem = battleSystem;
        }

        public override void Enter()
        {
            base.Enter();
            _animator.SetBool(_monsterBaseStateInfo.stateParameter, true);
            _movementSystem.SetRun(true);
            OnFixedUpdate += CheckTargetAndIdle;
        }
        public override void Exit()
        {
            base.Exit();
            _animator.SetBool(_monsterBaseStateInfo.stateParameter, false);
            _movementSystem.SetRun(false);
        }
        private void CheckTargetAndIdle()
        {
            if (_battleSystem.CheckCollider(_monsterRunStateInfo.targetLayer, _monsterRunStateInfo.direction, _monsterRunStateInfo.distance, out _))
            {
                OnFixedUpdate -= CheckTargetAndIdle;
                _tryChangeState.Invoke(StateType.Idle);
            }
        }
    }
}