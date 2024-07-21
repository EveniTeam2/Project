using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
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
            if (!_battleSystem.CheckCollider(_idleStateInfo.targetLayer, _idleStateInfo.direction, _idleStateInfo.distance, out _))
            {
                OnFixedUpdate -= CheckTargetAndRun;
                _tryChangeState.Invoke(StateType.Run);
            }
        }
    }

    public class MonsterIdleState : MonsterBaseState
    {
        protected BattleSystem _battleSystem;
        protected Animator _animator;

        protected readonly MonsterIdleStateInfo _monsterIdleStateInfo;

        public MonsterIdleState(MonsterBaseStateInfo baseInfo, MonsterIdleStateInfo monsterIdleStateInfo, Func<StateType, bool> tryChangeState, Animator animator, BattleSystem battleSystem, MonsterEventPublisher monsterEventPublisher)
            : base(baseInfo, tryChangeState, monsterEventPublisher)
        {
            _monsterIdleStateInfo = monsterIdleStateInfo;
            _animator = animator;
            _battleSystem = battleSystem;
        }

        public override void Enter()
        {
            base.Enter();
            _animator.SetBool(_monsterBaseStateInfo.stateParameter, true);
            OnFixedUpdate += CheckTargetAndRun;
        }
        public override void Exit()
        {
            base.Exit();
            _animator.SetBool(_monsterBaseStateInfo.stateParameter, false);
        }

        protected virtual void CheckTargetAndRun()
        {
            if (!_battleSystem.CheckCollider(_monsterIdleStateInfo.targetLayer, _monsterIdleStateInfo.direction, _monsterIdleStateInfo.distance, out _))
            {
                OnFixedUpdate -= CheckTargetAndRun;
                _tryChangeState.Invoke(StateType.Run);
            }
        }
    }
}