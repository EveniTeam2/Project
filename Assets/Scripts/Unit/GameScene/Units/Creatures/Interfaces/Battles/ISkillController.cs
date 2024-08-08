using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces.Battles
{
    public interface ISkillController
    {
        public void Attack(int value, float range);
        public void Heal(int value);
        public void Buff(StatType statType, int value, float duration);
        public bool CheckEnemyInRange(float distance, out RaycastHit2D[] collider);
    }
}