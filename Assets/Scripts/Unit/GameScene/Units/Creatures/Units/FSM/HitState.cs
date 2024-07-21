using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class HitState : BaseState
    {
        //protected readonly HitStateInfo _hitStateInfo;

        public HitState(BaseStateInfo baseStateInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver) : base(baseStateInfo, tryChangeState, animatorEventReceiver)
        {
            //this._hitStateInfo = hitStateInfo;
        }

        public override void Enter()
        {
            base.Enter();
            _animatorEventReceiver.SetTrigger(_baseStateInfo.stateParameter, null);
        }
        public override void Exit()
        {
            base.Exit();
        }
    }

    public class MonsterHitState : MonsterBaseState
    {
        //protected readonly HitStateInfo _hitStateInfo;
        private Animator _animator;

        public MonsterHitState(MonsterBaseStateInfo monsterBaseStateInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver) : base(monsterBaseStateInfo, tryChangeState, animatorEventReceiver)
        {
            //this._hitStateInfo = hitStateInfo;
        }

        public override void Enter()
        {
            base.Enter();
            _animator.SetTrigger(_monsterBaseStateInfo.stateParameter);
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}