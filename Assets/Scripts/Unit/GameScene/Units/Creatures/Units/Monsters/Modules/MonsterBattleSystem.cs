using Assets.Scripts.Unit.GameScene.Units.Creatures.Units;
using System;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules
{
    public class MonsterBattleSystem : BattleSystem
    {
        protected float timer;

        public MonsterBattleSystem(Transform targetTransform, MonsterBattleStat stat) : base(targetTransform, stat)
        {
        }

        public override void Attack(RaycastHit2D col)
        {
            _canAttackCool = false;
            timer = _stat.GetCoolTime();

            if (col.collider.gameObject.TryGetComponent<Character>(out var target))
            {
#if UNITY_EDITOR
                var dmg = target.GetServiceProvider().TakeDamage(_stat.GetAttack());
                Debug.Log($"몬스터가 {col.collider.gameObject.name}에게 {dmg} 피해를 입혔습니다.");
#else
                var dmg = target.GetServiceProvider().TakeDamage(_stat.GetAttack());
#endif
            }
            else
            {
                Debug.LogWarning($"몬스터가 {col.collider.gameObject.name}의 컴포넌트를 가지고 올 수 없습니다.");
            }
        }

        public override void Attack(RaycastHit2D col, IBattleEffect effect)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            if (!_canAttackCool)
            {
                timer -= Time.deltaTime;
                if (timer < 0) _canAttackCool = true;
            }
        }
    }


    public class MonsterBattleStat : IBattleStat
    {
        private readonly Func<int> _attack;
        private readonly Func<float> _coolTime;

        public MonsterBattleStat(Stat<MonsterStat> stat)
        {
            _attack = () => stat.Current.Attack;
            _coolTime = () => stat.Current.CoolTime;
        }

        public float GetCoolTime()
        {
            return _coolTime();
        }

        public int GetAttack()
        {
            return _attack();
        }
    }
}