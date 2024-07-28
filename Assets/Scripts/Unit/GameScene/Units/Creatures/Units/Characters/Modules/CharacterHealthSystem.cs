using System;
using Unit.GameScene.Units.Creatures.Module;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    public class CharacterHealthSystem : HealthSystem
    {
        public CharacterHealthSystem(CharacterHealthStat stats) : base(stats)
        {
        }

        public override void TakeDamage(int dmg)
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

        public override void TakeHeal(int healAmount)
        {
            _health += healAmount;
            CallHeal();
        }
    }

    public class CharacterHealthStat : IHealthStat
    {
        private readonly Func<int> _getHealth;
        private readonly Action<int> _setHealth;

        public CharacterHealthStat(Stat<CharacterStat> stat)
        {
            //TODO : 채이환
            _getHealth = () => stat.Current.MaxHp;
            _setHealth = value =>
            {
                var current = stat.Current;
                current.CurrentHp = value;
                stat.SetCurrent(current);
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