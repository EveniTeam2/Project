using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Module
{
    public abstract class BattleSystem
    {
        protected bool _canAttackCool = true;
        protected IBattleStat _stat;
        protected Transform _targetTransform;

        public BattleSystem(Transform targetTransform, IBattleStat stat)
        {
            _targetTransform = targetTransform;
            _stat = stat;
        }

        protected bool CanAttackCool => _canAttackCool;

        public bool CheckCollider(LayerMask targetLayer, Vector2 direction, float distance, out RaycastHit2D[] collidee)
        {
            var hits = Physics2D.RaycastAll(_targetTransform.position, direction, distance, targetLayer);
            collidee = hits;
            if (collidee.Length > 0)
                return true;
            return false;
        }

        public abstract void Attack(RaycastHit2D col);
        public abstract void Update();
    }

    public interface IBattleStat
    {
        float GetCoolTime();
        int GetAttack();
    }
}