using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public abstract class MonsterBaseState : BaseState
    {
        protected readonly MonsterBaseStateInfo monsterBaseStateInfo;
        protected readonly MonsterEventPublisher eventPublisher;

        protected MonsterBaseState(MonsterBaseStateInfo monsterBaseStateInfo,
                                   BaseStateInfo baseStateInfo,
                                   Func<StateType, bool> tryChangeState,
                                   MonsterEventPublisher eventPublisher) : base(baseStateInfo, tryChangeState)
        {
            this.monsterBaseStateInfo = monsterBaseStateInfo;
            this.eventPublisher = eventPublisher;
        }

        public override void Enter()
        {
            base.Enter();
            eventPublisher.RegistOnEvent(eEventType.AnimationEnd, ChangeToDefaultState);
        }

        public override void Exit()
        {
            base.Exit();
            eventPublisher.UnregistOnEvent(eEventType.AnimationEnd, ChangeToDefaultState);
        }

        private void ChangeToDefaultState()
        {
            _tryChangeState.Invoke(monsterBaseStateInfo._defaultExitState);
        }
    }
}