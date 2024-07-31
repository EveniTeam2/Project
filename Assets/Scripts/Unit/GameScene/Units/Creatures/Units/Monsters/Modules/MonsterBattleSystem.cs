using System;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module.Systems;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules
{
    public class MonsterBattleSystem : BattleSystem
    {
        private MonsterBattleStat BattleStat;
        protected float timer;

        public MonsterBattleSystem(Transform targetTransform, MonsterBattleStat battleStat) : base(targetTransform)
        {
            this.BattleStat = battleStat;
        }

        public override void Attack(RaycastHit2D col)
        {
            IsReadyForAttack = false;
            timer = BattleStat.GetCoolTime();

            if (col.collider.gameObject.TryGetComponent<ICreatureServiceProvider>(out var target))
            {
#if UNITY_EDITOR
                target.HeathSystem.TakeDamage(BattleStat.GetAttack());
                Debug.Log($"몬스터가 {col.collider.gameObject.name}에게 {BattleStat.GetAttack()} 피해를 입혔습니다.");
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
            if (col.collider.gameObject.TryGetComponent<Creature>(out var target))
            {
                effect.Attack(target);
            }
        }

        public override void Update()
        {
            if (!IsReadyForAttack)
            {
                timer -= Time.deltaTime;
                if (timer < 0) IsReadyForAttack = true;
            }
        }

        internal void SpawnInit(MonsterBattleStat monsterBattleStat)
        {
            BattleStat = monsterBattleStat;
        }
    }


    public class MonsterBattleStat
    {
        private readonly Func<int> _attack;
        private readonly Func<float> _coolTime;

        public MonsterBattleStat(CreatureStat<MonsterStat> creatureStat)
        {
            _attack = () => creatureStat.Current.Attack;
            _coolTime = () => creatureStat.Current.CoolTime;
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