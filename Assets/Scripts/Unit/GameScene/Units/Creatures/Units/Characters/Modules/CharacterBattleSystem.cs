using Assets.Scripts.Unit.GameScene.Units.Creatures.Units;
using System;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Monsters;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    public class CharacterBattleSystem : BattleSystem
    {
        public CharacterBattleSystem(Transform targetTransform, CharacterBattleStat stat) : base(targetTransform, stat)
        {
        }

        public override void Attack(RaycastHit2D col)
        {
            if (col.collider.gameObject.TryGetComponent<Monster>(out var target))
            {
#if UNITY_EDITOR
                var dmg = target.GetServiceProvider().TakeDamage(_stat.GetAttack());
                Debug.Log($"플레이어가 {col.collider.gameObject.name}에게 {dmg} 피해를 입혔습니다.");
#else
                var dmg = target.GetServiceProvider().TakeDamage(_stat.GetAttack());
#endif
            }
            else
            {
                Debug.LogWarning($"플레이어가 {col.collider.gameObject.name}의 컴포넌트를 가지고 올 수 없습니다.");
            }
        }

        public override void Attack(RaycastHit2D col, IBattleEffect effect)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
        }
    }

    public class CharacterBattleStat : IBattleStat
    {
        private readonly Func<int> _attack;
        private readonly Func<float> _cool;

        public CharacterBattleStat(Stat<CharacterStat> stat)
        {
            _attack = () => stat.Current.Damage;
            _cool = () => stat.Current.CoolTime;
        }

        public int GetAttack()
        {
            return _attack();
        }

        public float GetCoolTime()
        {
            return _cool();
        }
    }
}