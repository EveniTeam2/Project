using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class MonsterSkillMeleeAttack : IMonsterSkillAct
    {
        private SkillAttackInfo _skillAttackInfo;
        private readonly IMonsterFsmController _fsmController;

        public MonsterSkillMeleeAttack(SkillAttackInfo skillAttackInfo, IMonsterFsmController fsmController)
        {
            _skillAttackInfo = skillAttackInfo;
            _fsmController = fsmController;
        }

        public void Act(MonsterBattleStat stat, RaycastHit2D target)
        {
            // BattleSystem 의 Attack(target, BattleEffect)로 변경할 예정
            _fsmController.Attack(target);
        }
    }
}