using System;
using Unit.GameScene.Stages.Creautres.Characters;
using Unit.GameScene.Stages.Creautres.Characters.Unit.Character;
using Unit.GameScene.Stages.Creautres.Interfaces;
using Unit.GameScene.Stages.Creautres.Monsters;
using UnityEngine;

namespace Unit.GameScene.Stages.Creautres.FSM.ActOnInput
{
    public class HealthSystem : IDamageable
    {
        private readonly BaseCreature _character;
        private readonly Func<int> GetHeatlh;
        private readonly Action<int> SetHealth;

        public HealthSystem(PlayerCreature character, Stat<CharacterStat> stats)
        {
            _character = character;
            GetHeatlh = () => stats.Current.Health;
            SetHealth = value =>
            {
                var cur = stats.Current;
                cur.Health -= value;
                stats.SetCurrent(cur);
            };
        }

        public HealthSystem(MonsterCreature monster, Stat<MonsterStat> stats)
        {
            _character = monster;
            GetHeatlh = () => stats.Current.Health;
        }

        private bool _isDead => _health <= 0;
        private int _health => GetHeatlh.Invoke();
        int IDamageable.Health => _health;
        bool IDamageable.IsDead => _isDead;

        event Action<BaseCreature> IDamageable.OnDeath
        {
            add => _onDeath += value;
            remove => _onDeath -= value;
        }

        event Action<BaseCreature> IDamageable.OnDamage
        {
            add => _onDamage += value;
            remove => _onDamage -= value;
        }

        public void Damage(int dmg)
        {
            // TODO 방어력 있으면 적용해야되는 곳
            Debug.Log($"{_character.name} {dmg} 데미지 적용");
            SetHealth.Invoke(_health - dmg);
            if (_health < 0)
            {
                SetHealth.Invoke(0);
                _onDeath?.Invoke(_character);
            }

            _onDamage?.Invoke(_character);
        }

        private event Action<BaseCreature> _onDeath;
        private event Action<BaseCreature> _onDamage;
    }

    public class HealthStat
    {
        private readonly Func<int> GetHealth;

        public HealthStat(Stat<CharacterStat> stat)
        {
            GetHealth = () => stat.Current.Health;
        }

        public HealthStat(Stat<MonsterStat> stat)
        {
            GetHealth = () => stat.Current.Health;
        }

        // TODO 방어력 있으면 적용해야되는 곳
        public int Health => GetHealth.Invoke();
    }
}