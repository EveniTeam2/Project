using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.Creatures.Units.FSM
{
    public class MonsterSkillState : MonsterBaseState
    {
        protected BattleSystem _battleSystem;
        MonsterSkillStateInfo _skillInfo;
        IBattleStat _stat;

        public MonsterSkillState(MonsterBaseStateInfo monsterBaseStateInfo,
                                 MonsterSkillStateInfo skillInfo,
                                 Func<StateType, bool> tryChangeState,
                                 BattleSystem battleSystem,
                                 AnimatorEventReceiver animatorEventReceiver)
            : base(monsterBaseStateInfo, tryChangeState, animatorEventReceiver)
        {
            _skillInfo = skillInfo;
            _battleSystem = battleSystem;
            _stat = _battleSystem.GetBattleStat();
        }

        public override void Enter()
        {
            base.Enter();
            animatorEventReceiver.SetBool(_monsterBaseStateInfo.stateParameter, true, ChangeToDefaultState);
            animatorEventReceiver.SetInteger(_skillInfo.skillParameter, _skillInfo.skillValue, null);
            animatorEventReceiver.OnAttack += AttackEvent;
        }

        public override void Exit()
        {
            base.Exit();
            animatorEventReceiver.SetBool(_monsterBaseStateInfo.stateParameter, false, null);
            animatorEventReceiver.OnAttack -= AttackEvent;
        }

        private void AttackEvent()
        {
            if (_battleSystem.CheckCollider(_skillInfo.targetLayer,
                                            _skillInfo.direction,
                                            _skillInfo.distance,
                                            out var targets))
            {
                foreach (var target in targets)
                    _skillInfo.skillAct.Act(_stat, target);
            }
        }
    }
}