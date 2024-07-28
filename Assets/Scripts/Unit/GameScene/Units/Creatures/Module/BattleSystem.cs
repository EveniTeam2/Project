using Assets.Scripts.Unit.GameScene.Units.Creatures.Units;
using Unit.GameScene.Units.Creatures.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module
{
    public abstract class BattleSystem : ICreatureBattle
    {
        protected bool _canAttackCool = true;
        protected IBattleStat _stat;
        protected Transform _targetTransform;

        public BattleSystem(Transform targetTransform, IBattleStat stat)
        {
            _targetTransform = targetTransform;
            _stat = stat;
        }

        public bool CanAttackCool => _canAttackCool;

        public bool CheckCollider(LayerMask targetLayer, Vector2 direction, float distance, out RaycastHit2D[] collidee)
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

        public bool CheckCollider(LayerMask targetLayer, Vector2 startPosition, Vector2 direction, float distance, out RaycastHit2D[] collidee)
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
        public void Attack(int damage, float range)
        {
            //TODO : 채이환
        }

        public abstract void Update();

        internal void SpawnInit(IBattleStat stat)
        {
            _stat = stat;
        }

        internal IBattleStat GetBattleStat()
        {
            return _stat;
        }
    }

    public interface IBattleStat
    {
        float GetCoolTime();
        int GetAttack();
    }
}