using System;
using Unit.GameScene.Stages.Creatures.Module;

namespace Unit.GameScene.Stages.Creatures.Units.Monsters.Modules
{
    public class MonsterHealthSystem : HealthSystem
    {
        public MonsterHealthSystem(MonsterHealthStat stats) : base(stats)
        {
        }

        public override void Damage(int dmg)
        {
            // TODO 방어력 있으면 적용해야되는 곳
            _health = dmg;
            if (_health <= 0)
            {
                _health = 0;
                CallDeath();
            }

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

        public MonsterHealthStat(Stat<MonsterStat> stat)
        {
            _getHealth = () => stat.Current.Health;
            _setHealth = value => stat.Current.Health = value;
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