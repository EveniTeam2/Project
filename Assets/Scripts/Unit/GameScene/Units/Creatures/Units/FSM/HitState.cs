using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class HitState : BaseState
    {
        //protected readonly HitStateInfo _hitStateInfo;
        private Animator _animator;

        public HitState(BaseStateInfo baseStateInfo, Func<StateType, bool> tryChangeState) : base(baseStateInfo, tryChangeState)
        {
            //this._hitStateInfo = hitStateInfo;
        }

        public override void Enter()
        {
            base.Enter();
            _animator.SetTrigger(_baseStateInfo.stateParameter);
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

        public MonsterHitState(MonsterBaseStateInfo monsterBaseStateInfo, Func<StateType, bool> tryChangeState, MonsterEventPublisher monsterEventPublisher) : base(monsterBaseStateInfo, tryChangeState, monsterEventPublisher)
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