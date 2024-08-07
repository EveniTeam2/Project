using System;
using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems;
using Unit.GameScene.Units.FSMs.Units.Monster.Structs;
using UnityEngine;

namespace Unit.GameScene.Units.FSMs.Units.Monster.States
{
    public class MonsterSkillState : MonsterBaseState
    {
        private readonly MonsterSkillStateInfo[] _skillInfo;
        private readonly MonsterStatSystem _stat;
        private MonsterSkillStateInfo targetSkill;

        public MonsterSkillState(MonsterBaseStateInfo monsterBaseStateInfo, MonsterSkillStateInfo[] skillInfo, Func<StateType, bool> tryChangeState, IMonsterFsmController fsmController, MonsterStatSystem stat)
            : base(monsterBaseStateInfo, tryChangeState, fsmController)
        {
            _skillInfo = skillInfo;
            _stat = stat;
        }

        public override void Enter()
        {
            base.Enter();

            foreach (var skillInfo in _skillInfo)
            {
                if (skillInfo.SkillDecider.CanExcute())
                {
                    targetSkill = skillInfo;
                    break;
                }
            }

            FsmController.RegisterOnAttackEventHandler(OnAttack);
            FsmController.SetBool(MonsterBaseStateInfo.StateParameter, true, ChangeToDefaultState);
            FsmController.SetFloat(targetSkill.SkillParameter, targetSkill.SkillValue, null);
        }

        public override void Exit()
        {
            base.Exit();

            FsmController.UnregisterOnAttackEventHandler(OnAttack);
            FsmController.SetBool(MonsterBaseStateInfo.StateParameter, false, null);
        }

        private void OnAttack()
        {
            if (!FsmController.CheckEnemyInRange(targetSkill.TargetLayer, targetSkill.Direction, targetSkill.Distance, out RaycastHit2D[] targets))
                return;

            foreach (RaycastHit2D target in targets)
            {
                targetSkill.SkillAct.Act(_stat, target);
            }
        }
    }
}