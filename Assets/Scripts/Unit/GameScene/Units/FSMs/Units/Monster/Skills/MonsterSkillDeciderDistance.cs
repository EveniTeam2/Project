using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    public class MonsterSkillDeciderDistance : IMonsterSkillDecider
    {
        private readonly IMonsterFsmController monsterController;
        private readonly IMonsterSkillAct targetSkill;
        private readonly int distance;
        private LayerMask targetLayer;
        private Vector2 direction;

        public MonsterSkillDeciderDistance(IMonsterFsmController monsterController, IMonsterSkillAct targetSkill, int distance, LayerMask targetLayer, Vector2 direction)
        {
            this.monsterController = monsterController;
            this.targetSkill = targetSkill;
            this.distance = distance;
            this.targetLayer = targetLayer;
            this.direction = direction;
        }

        bool IMonsterSkillDecider.CanExcute()
        {
            return monsterController.CheckEnemyInRange(targetLayer, direction, distance, out _);
        }

        void IMonsterSkillDecider.ResetDecider()
        {
            
        }
    }
}