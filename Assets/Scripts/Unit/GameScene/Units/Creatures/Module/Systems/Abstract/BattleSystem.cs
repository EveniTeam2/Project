using Unit.GameScene.Units.Creatures.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.Systems.Abstract
{
    public abstract class BattleSystem : ICreatureBattle
    {
        public bool IsReadyForAttack { get; protected set; } = true;
        private readonly Transform _targetTransform;

        protected BattleSystem(Transform targetTransform)
        {
            _targetTransform = targetTransform;
        }

        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float distance, out RaycastHit2D[] collidee)
        {
            var hits = Physics2D.RaycastAll(_targetTransform.position, direction, distance, targetLayer);
            collidee = hits;
            if (collidee.Length > 0)
            {
#if UNITY_EDITOR
                Debug.DrawRay(_targetTransform.position, distance * direction, Color.red, 0.3f);
#endif
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.DrawRay(_targetTransform.position, distance * direction, Color.green, 0.3f);
#endif
                return false;
            }
        }

        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 startPosition, Vector2 direction, float distance, out RaycastHit2D[] collidee)
        {
            var hits = Physics2D.RaycastAll(startPosition, direction, distance, targetLayer);
            collidee = hits;
            if (collidee.Length > 0)
            {
#if UNITY_EDITOR
                Debug.DrawRay(startPosition, distance * direction, Color.red, 0.3f);
#endif
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.DrawRay(startPosition, distance * direction, Color.green, 0.3f);
#endif
                return false;
            }
        }

        public abstract void Attack(RaycastHit2D col);
        public abstract void Attack(RaycastHit2D col, IBattleEffect effect);

        public abstract void Update();

        public void Attack(int damage, float range)
        {
            if (CheckEnemyInRange(1 << LayerMask.NameToLayer("Monster"), Vector2.right, range, out var targets))
            {
                foreach (var target in targets)
                    Attack(target, new BattleEffect(damage));
            }
        }
    }
}