using Unit.GameScene.Units.Creatures.Interfaces.Battles;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public abstract class BattleSystem : ICreatureBattle
    {
        protected LayerMask TargetLayerMask;
        protected Vector3 TargetDirection;
        
        private readonly Transform _targetTransform;
        
        protected abstract void SendDamage(RaycastHit2D target, int damage);

        protected BattleSystem(Transform targetTransform)
        {
            _targetTransform = targetTransform;
        }

        public abstract void Update();
        
        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float distance, out RaycastHit2D[] collider)
        {
            var rayPos = new Vector3(_targetTransform.position.x, _targetTransform.position.y + 1, _targetTransform.position.z);
            var hits = Physics2D.RaycastAll(rayPos, direction, distance, targetLayer);
            
            collider = hits;
            
            if (collider.Length > 0)
            {
#if UNITY_EDITOR
                Debug.DrawRay(new Vector3(rayPos.x, rayPos.y, rayPos.z), distance * direction, Color.red, 0.5f);
#endif
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.DrawRay(new Vector3(rayPos.x, rayPos.y, rayPos.z), distance * direction, Color.green, 0.5f);
#endif
                return false;
            }
        }
        
        public void AttackEnemy(int damage, float range)
        {
            if (!CheckEnemyInRange(TargetLayerMask, TargetDirection, range, out var targets)) return;
            
            foreach (var target in targets)
            {
                SendDamage(target, damage);
            }
        }

        public void AttackEnemy(int damage, RaycastHit2D target)
        {
            SendDamage(target, damage);
        }
    }
}