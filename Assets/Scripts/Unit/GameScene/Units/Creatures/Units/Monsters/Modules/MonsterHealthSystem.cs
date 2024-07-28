using System;
using Unit.GameScene.Units.Creatures.Module;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules
{
    public class MonsterHealthSystem : HealthSystem
    {
        public MonsterHealthSystem(MonsterHealthStat stats) : base(stats)
        {
        }

        public override void Damage(int dmg)
        {
            if (_invinsible)
                return;

            _health -= dmg;
            if (_health <= 0)
            {
                _health = 0;
                CallDeath();
            }
            else
                CallDamage();
        }

        public override void Heal(int healAmount)
        {
            _health += healAmount;
            CallHeal();
        }
    }

    public class MonsterHealthStat : IHealthStat
    {
        private readonly Func<int> _getHealth;
        private readonly Action<int> _setHealth;

        public MonsterHealthStat(CreatureStat<MonsterStat> creatureStat)
        {
            _getHealth = () => creatureStat.Current.Health;
            _setHealth = value =>
            {
                var current = creatureStat.Current;
                current.Health = value;
                creatureStat.SetCurrent(current);
            };
        }

        public int GetHealth()
        {
            return _getHealth.Invoke();
        }

        public void SetHealth(int health)
        {
            _setHealth?.Invoke(health);
        }
    }
}