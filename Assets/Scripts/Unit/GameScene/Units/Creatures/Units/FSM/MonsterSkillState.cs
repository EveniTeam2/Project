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
        protected BattleSystem _battleSystem;
        MonsterSkillStateInfo _skillInfo;

        public MonsterSkillState(MonsterBaseStateInfo monsterBaseStateInfo,
                                 MonsterSkillStateInfo skillInfo,
                                 Func<StateType, bool> tryChangeState,
                                 BattleSystem battleSystem,
                                 Animator animator,
                                 MonsterEventPublisher eventPublisher)
            : base(monsterBaseStateInfo, tryChangeState, eventPublisher)
        {
            _skillInfo = skillInfo;
            _battleSystem = battleSystem;
            _animator = animator;
        }

        public override void Enter()
        {
            base.Enter();
            _animator.SetBool(_monsterBaseStateInfo.stateParameter, true);
            _eventPublisher.RegistOnAttackEvent(AttackEvent);
        }

        public override void Exit()
        {
            base.Exit();
            _animator.SetBool(_monsterBaseStateInfo.stateParameter, false);
            _eventPublisher.UnregistOnAttackEvent(AttackEvent);
        }

        private void AttackEvent(IBattleStat stat)
        {
            if (_battleSystem.CheckCollider(_skillInfo.targetLayer,
                                            _skillInfo.direction,
                                            _skillInfo.distance,
                                            out var targets))
            {
                foreach (var target in targets)
                    _skillInfo.skillAct.Act(stat, target);
            }
        }
    }
}