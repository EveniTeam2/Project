using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.Creatures.Units.FSM
{
    public class RunState : BaseState
    {
        protected RunStateInfo _runStateInfo;
        private MovementSystem _movementSystem;
        private BattleSystem _battleSystem;

        public RunState(BaseStateInfo baseInfo, RunStateInfo runStateInfo, Func<StateType, bool> tryChangeState, BattleSystem battleSystem, MovementSystem movementSystem, AnimatorEventReceiver animatorEventReceiver) : base(baseInfo, tryChangeState, animatorEventReceiver)
        {
            _runStateInfo = runStateInfo;
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
            OnFixedUpdate -= CheckTargetAndIdle;
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
        private MovementSystem _movementSystem;
        private BattleSystem _battleSystem;

        public MonsterRunState(MonsterBaseStateInfo monsterBaseInfo,
                               MonsterRunStateInfo monsterRunStateInfo,
                               Func<StateType, bool> tryChangeState,
                               BattleSystem battleSystem,
                               MovementSystem movementSystem,
                               AnimatorEventReceiver animatorEventReceiver) : base(monsterBaseInfo, tryChangeState, animatorEventReceiver)
        {
            _monsterRunStateInfo = monsterRunStateInfo;
            _movementSystem = movementSystem;
            _battleSystem = battleSystem;
        }

        public override void Enter()
        {
            base.Enter();
            animatorEventReceiver.SetBool(_monsterBaseStateInfo.stateParameter, true, null);
            _movementSystem.SetRun(true);
            OnFixedUpdate += CheckTargetAndIdle;
        }
        public override void Exit()
        {
            base.Exit();
            animatorEventReceiver.SetBool(_monsterBaseStateInfo.stateParameter, false, null);
            _movementSystem.SetRun(false);
            OnFixedUpdate -= CheckTargetAndIdle;
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