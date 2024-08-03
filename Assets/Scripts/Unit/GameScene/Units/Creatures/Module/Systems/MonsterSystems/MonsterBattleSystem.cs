using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems
{
    public class MonsterBattleSystem : BattleSystem
    {
        public bool IsReadyForAttack { get; private set; } = true;
        
        private readonly MonsterStatSystem _monsterStatSystem;
        
        private float _attackCoolTimer;

        public MonsterBattleSystem(MonsterStatSystem monsterStatSystem, Transform targetTransform)
            : base(targetTransform)
        {
            TargetLayerMask = 1 << 6;
            TargetDirection = Vector3.left;
            _monsterStatSystem = monsterStatSystem;
        }
        
        protected override void SendDamage(RaycastHit2D target, int damage)
        {
            IsReadyForAttack = false;
            _attackCoolTimer = _monsterStatSystem.AttackCoolTime;

            if (target.collider.gameObject.TryGetComponent<ITakeMonsterDamage>(out var targetObject))
            {
                targetObject.TakeDamage(damage);
#if UNITY_EDITOR
                Debug.Log($"몬스터가 {target.collider.gameObject.name}에게 {damage} 피해를 입혔습니다.");
#endif
            }
            else
            {
                Debug.LogWarning($"몬스터가 {target.collider.gameObject.name}의 컴포넌트를 가지고 올 수 없습니다.");
            }
        }

        public override void Update()
        {
            if (IsReadyForAttack) return;
            
            _attackCoolTimer -= Time.deltaTime;

            if (_attackCoolTimer < 0) IsReadyForAttack = true;
        }
    }
}