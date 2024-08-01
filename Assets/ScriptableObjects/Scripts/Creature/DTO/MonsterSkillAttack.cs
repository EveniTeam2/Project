using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class MonsterSkillAttack : IMonsterSkillAct
    {
        private SkillAttackInfo _skillAttackInfo;
        private readonly IMonsterFsmController _fsmController;

        public MonsterSkillAttack(SkillAttackInfo skillAttackInfo, IMonsterFsmController fsmController)
        {
            _skillAttackInfo = skillAttackInfo;
            _fsmController = fsmController;
        }

        public void Act(RaycastHit2D target)
        {
            _fsmController.AttackEnemy(target);
        }
    }
}