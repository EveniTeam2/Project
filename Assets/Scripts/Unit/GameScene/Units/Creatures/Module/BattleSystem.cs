using System;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
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

        public abstract void Attack(RaycastHit2D col);
        public abstract void Update();

        internal void SpawnInit(IBattleStat stat)
        {
            _stat = stat;
        }
    }

    public interface IBattleStat
    {
        float GetCoolTime();
        int GetAttack();
    }
}