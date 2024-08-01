using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces.Battles
{
    public interface ICreatureBattle
    {
        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float distance, out RaycastHit2D[] collider);
        public void AttackEnemy(int damage, float range);
    }
}