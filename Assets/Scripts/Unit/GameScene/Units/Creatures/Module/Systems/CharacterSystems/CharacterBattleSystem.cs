using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.Battles;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems
{
    public class CharacterBattleSystem : BattleSystem
    {
        private readonly CharacterStatSystem _characterStatSystem;
        
        public CharacterBattleSystem(CharacterStatSystem characterStatSystem, Transform targetTransform)
            : base(targetTransform)
        {
            TargetLayerMask = 1 << 7;
            TargetDirection = Vector3.right;
            _characterStatSystem = characterStatSystem;
        }

        protected override void SendDamage(RaycastHit2D target, int damage)
        {
            var finalDamage = damage * _characterStatSystem.Damage;
            
            if (target.collider.gameObject.TryGetComponent<ITakePlayerDamage>(out var targetObject))
            {
                var damageDelegate = targetObject.TakeDamage();
                int exp = damageDelegate(finalDamage);

                if (exp > 0) _characterStatSystem.OnIncreasePlayerExp.Invoke(exp);
                
#if UNITY_EDITOR
                Debug.Log($"플레이어가 {target.collider.gameObject.name}에게 {finalDamage} 피해를 입혔습니다.");
#endif
            }
            else
            {
                Debug.LogWarning($"플레이어가 {target.collider.gameObject.name}의 컴포넌트를 가지고 올 수 없습니다.");
            }
        }
    }
}