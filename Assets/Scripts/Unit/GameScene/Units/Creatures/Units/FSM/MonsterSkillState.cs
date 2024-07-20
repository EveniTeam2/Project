using System;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using Unit.GameScene.Stages.Creatures.Units.Monsters.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class MonsterSkillState : MonsterBaseState
    {
        protected Animator _animator;
        protected readonly MonsterEventPublisher _eventPublisher;
        protected BattleSystem _battleSystem;
        MonsterSkillStateInfo _skillInfo;

        public MonsterSkillState(MonsterSkillStateInfo skillInfo,
                                 MonsterBaseStateInfo monsterBaseStateInfo,
                                 BaseStateInfo baseInfo,
                                 Func<StateType, bool> tryChangeState,
                                 BattleSystem battleSystem,
                                 Animator animator,
                                 MonsterEventPublisher eventPublisher)
            : base(monsterBaseStateInfo, baseInfo, tryChangeState, eventPublisher)
        {
            _skillInfo = skillInfo;
            _battleSystem = battleSystem;
            _animator = animator;
            _eventPublisher = eventPublisher;
        }

        public override void Enter()
        {
            base.Enter();
            _animator.SetBool(_baseStateInfo.stateParameter, true);
            _eventPublisher.RegistOnAttackEvent(AttackEvent);
            _eventPublisher.RegistOnEvent(eEventType.AnimationEnd, AnimationEnd);
        }

        public override void Exit()
        {
            base.Exit();
            _animator.SetBool(_baseStateInfo.stateParameter, false);
            _eventPublisher.UnregistOnAttackEvent(AttackEvent);
            _eventPublisher.UnregistOnEvent(eEventType.AnimationEnd, AnimationEnd);
        }

        private void AttackEvent(IBattleStat stat)
        {
            if (_battleSystem.CheckCollider(_skillInfo.targetLayer,
                                            _skillInfo.direction,
                                            _skillInfo.distance,
                                            out var targets))
            {
                foreach (var target in targets)
                    _battleSystem.Attack(target);
            }
        }

        private void AnimationEnd()
        {
            _tryChangeState.Invoke(monsterBaseStateInfo._defaultExitState);
        }
    }
}