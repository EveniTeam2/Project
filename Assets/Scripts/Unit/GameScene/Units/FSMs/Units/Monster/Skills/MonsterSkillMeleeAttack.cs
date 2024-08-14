using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using System;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class MonsterSkillMeleeAttack : IMonsterSkillAct
    {
        private SkillAttackInfo _skillAttackInfo;
        private readonly IMonsterFsmController _fsmController;
        public event Action OnExcute;

        public MonsterSkillMeleeAttack(SkillAttackInfo skillAttackInfo, IMonsterFsmController fsmController)
        {
            _skillAttackInfo = skillAttackInfo;
            _fsmController = fsmController;
        }

        public void Act(MonsterStatSystem stat, RaycastHit2D target)
        {
            OnExcute?.Invoke();
            _fsmController.AttackEnemy(target);
        }
    }
}