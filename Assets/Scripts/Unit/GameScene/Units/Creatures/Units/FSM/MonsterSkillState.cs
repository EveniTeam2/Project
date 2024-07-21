using System;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using Unit.GameScene.Stages.Creatures.Units.Monsters.Modules;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class MonsterSkillState : MonsterBaseState
    {
        protected BattleSystem _battleSystem;
        MonsterSkillStateInfo _skillInfo;

        public MonsterSkillState(MonsterBaseStateInfo monsterBaseStateInfo,
                                 MonsterSkillStateInfo skillInfo,
                                 Func<StateType, bool> tryChangeState,
                                 BattleSystem battleSystem,
                                 AnimatorEventReceiver animatorEventReceiver)
            : base(monsterBaseStateInfo, tryChangeState, animatorEventReceiver)
        {
            _skillInfo = skillInfo;
            _battleSystem = battleSystem;
        }

        public override void Enter()
        {
            base.Enter();
            animatorEventReceiver.SetBool(_monsterBaseStateInfo.stateParameter, true, ChangeToDefaultState);
        }

        public override void Exit()
        {
            base.Exit();
            animatorEventReceiver.SetBool(_monsterBaseStateInfo.stateParameter, false, null);
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