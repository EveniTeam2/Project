using System;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules.Unit.Character;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Modules
{
    public class CharacterHealthSystem : HealthSystem
    {
        public CharacterHealthSystem(CharacterHealthStat stats) : base(stats)
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

    public class CharacterHealthStat : IHealthStat
    {
        private readonly Func<int> _getHealth;
        private readonly Action<int> _setHealth;

        public CharacterHealthStat(Stat<CharacterStat> stat)
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