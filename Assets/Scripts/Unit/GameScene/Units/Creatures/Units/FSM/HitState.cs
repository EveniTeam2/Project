using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.Creatures.Units.FSM
{
    public class HitState : BaseState
    {
        protected readonly HitStateInfo _hitStateInfo;

        public HitState(BaseStateInfo baseStateInfo, HitStateInfo hitStateInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver) : base(baseStateInfo, tryChangeState, animatorEventReceiver)
        {
            this._hitStateInfo = hitStateInfo;
        }

        public override void Enter()
        {
            base.Enter();
            _animatorEventReceiver.SetTrigger(_baseStateInfo.stateParameter, ChangeToDefaultState);
        }
        public override void Exit()
        {
            base.Exit();
        }

        private void ChangeToDefaultState()
        {
            _tryChangeState.Invoke(_hitStateInfo._defaultStateType);
        }
    }

    public class MonsterHitState : MonsterBaseState
    {
        //protected readonly HitStateInfo _hitStateInfo;

        public MonsterHitState(MonsterBaseStateInfo monsterBaseStateInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver) : base(monsterBaseStateInfo, tryChangeState, animatorEventReceiver)
        {
            //this._hitStateInfo = hitStateInfo;
        }

        public override void Enter()
        {
            base.Enter();
            animatorEventReceiver.SetTrigger(_monsterBaseStateInfo.stateParameter, ChangeToDefaultState);
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}