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

            if (!CheckSkillDecider())
            {
                foreach (var skill in _skillInfo)
                    skill.SkillDecider.ResetDecider();
                if (!CheckSkillDecider())
                {
                    Debug.Log("Monster Skill State에서 사용가능한 스킬이 없습니다!");
                    this.ChangeToDefaultState();
                }
            }
            FsmController.RegisterOnAttackEventHandler(OnAttack);
            FsmController.SetBool(MonsterBaseStateInfo.StateParameter, true, ChangeToDefaultState);
            FsmController.SetFloat(targetSkill.SkillParameter, targetSkill.SkillValue, null);
        }

        private bool CheckSkillDecider()
        {
            for (int i = 0; i < _skillInfo.Length; ++i)
            {
                if (_skillInfo[i].SkillDecider.CanExcute())
                {
                    SetSkill(_skillInfo[i]);
                    return true;
                }
            }
            return false;
        }

        private void SetSkill(MonsterSkillStateInfo monsterSkillStateInfo)
        {
            targetSkill = monsterSkillStateInfo;
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