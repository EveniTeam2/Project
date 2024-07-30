using Unit.GameScene.Units.Creatures.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.Systems.Abstract
{
    public abstract class BattleSystem : ICreatureBattle
    {
        public bool IsReadyForAttack { get; protected set; } = true;
        
        protected IBattleStat BattleStat;
        private readonly Transform _targetTransform;

        protected BattleSystem(Transform targetTransform, IBattleStat battleStat)
        {
            _targetTransform = targetTransform;
            BattleStat = battleStat;
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
        public void Attack(int damage, float range)
        {
            //TODO : 채이환
        }

        public abstract void Update();

        internal void SpawnInit(IBattleStat stat)
        {
            BattleStat = stat;
        }

        internal IBattleStat GetBattleStat()
        {
            return BattleStat;
        }
    }

    public interface IBattleStat
    {
        float GetCoolTime();
        int GetAttack();
        
        int GetSkillIndex(string skillName);
        float GetSkillRange(string skillName);
        int GetSkillValue(string skillName);
    }
}