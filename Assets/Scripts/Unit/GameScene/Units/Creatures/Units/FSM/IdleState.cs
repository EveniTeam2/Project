using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.Creatures.Units.FSM
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
            OnFixedUpdate -= CheckTargetAndRun;
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
            if (!_battleSystem.CheckCollider(_monsterIdleStateInfo.targetLayer, _monsterIdleStateInfo.direction, _monsterIdleStateInfo.distance, out var targets))
            {
                OnFixedUpdate -= CheckTargetAndRun;
                _tryChangeState.Invoke(StateType.Run);
            }
            else
            {
                if (_battleSystem.CanAttackCool)
                    _tryChangeState.Invoke(StateType.Skill);
            }
        }
    }
}