using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class IdleState : BaseState
    {
        protected BattleSystem _battleSystem;
        protected readonly IdleStateInfo _idleStateInfo;

        public IdleState(BaseStateInfo baseInfo, IdleStateInfo idleStateInfo, Func<StateType, bool> tryChangeState, BattleSystem battleSystem, AnimatorEventReceiver animatorEventReceiver)
            : base(baseInfo, tryChangeState, animatorEventReceiver)
        {
            _idleStateInfo = idleStateInfo;
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

        protected readonly MonsterIdleStateInfo _monsterIdleStateInfo;

        public MonsterIdleState(MonsterBaseStateInfo baseInfo, MonsterIdleStateInfo monsterIdleStateInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver, BattleSystem battleSystem)
            : base(baseInfo, tryChangeState, animatorEventReceiver)
        {
            _monsterIdleStateInfo = monsterIdleStateInfo;
            _battleSystem = battleSystem;
        }

        public override void Enter()
        {
            base.Enter();
            animatorEventReceiver.SetBool(_monsterBaseStateInfo.stateParameter, true, null);
            OnFixedUpdate += CheckTargetAndRun;
        }
        public override void Exit()
        {
            base.Exit();
            animatorEventReceiver.SetBool(_monsterBaseStateInfo.stateParameter, false, null);
            OnFixedUpdate -= CheckTargetAndRun;
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