using System;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Units.Monster.Structs;

namespace Unit.GameScene.Units.FSMs.Units.Monster.States
{
    public class MonsterSkillState : MonsterBaseState
    {
        private readonly MonsterSkillStateInfo _skillInfo;

        public MonsterSkillState(MonsterBaseStateInfo monsterBaseStateInfo, MonsterSkillStateInfo skillInfo, Func<StateType, bool> tryChangeState, IMonsterFsmController fsmController)
            : base(monsterBaseStateInfo, tryChangeState, fsmController)
        {
            _skillInfo = skillInfo;
        }

        public override void Enter()
        {
            base.Enter();
            
            FsmController.RegisterOnAttackEventHandler(OnAttack);
            FsmController.SetBool(MonsterBaseStateInfo.StateParameter, true, ChangeToDefaultState);
            FsmController.SetInteger(_skillInfo.SkillParameter, _skillInfo.SkillValue, null);
        }

        public override void Exit()
        {
            base.Exit();
            
            FsmController.UnregisterOnAttackEventHandler(OnAttack);
            FsmController.SetBool(MonsterBaseStateInfo.StateParameter, false, null);
        }

        private void OnAttack()
        {
            if (!FsmController.CheckEnemyInRange(_skillInfo.TargetLayer, _skillInfo.Direction, _skillInfo.Distance, out var targets)) return;
            
            foreach (var target in targets)
            {
                _skillInfo.SkillAct.Act(target);   
            }
        }
    }
}