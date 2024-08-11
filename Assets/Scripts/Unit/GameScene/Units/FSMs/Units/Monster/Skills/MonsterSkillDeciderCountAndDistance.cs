using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    public class MonsterSkillDeciderCountAndDistance : IMonsterSkillDecider
    {
        private readonly IMonsterFsmController monsterController;
        private readonly IMonsterSkillAct targetSkill;
        private readonly int count;
        private readonly float distance;
        private LayerMask targetLayer;
        private Vector2 direction;

        private int current;

        public MonsterSkillDeciderCountAndDistance(IMonsterFsmController monsterController, IMonsterSkillAct targetSkill, int count, float distance, LayerMask targetLayer, Vector2 direction)
        {
            this.monsterController = monsterController;
            this.targetSkill = targetSkill;
            this.count = count;
            this.distance = distance;
            this.targetLayer = targetLayer;
            this.direction = direction;
            targetSkill.OnExcute += Count;
        }

        private void Count()
        {
            ++current;
        }

        bool IMonsterSkillDecider.CanExcute()
        {
            if (current < count)
            {
                return monsterController.CheckEnemyInRange(targetLayer, direction, distance, out _);
            }
            return false;
        }

        void IMonsterSkillDecider.ResetDecider()
        {
            current = 0;
        }
    }
}