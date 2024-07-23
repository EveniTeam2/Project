using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.Creatures.Units.FSM
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