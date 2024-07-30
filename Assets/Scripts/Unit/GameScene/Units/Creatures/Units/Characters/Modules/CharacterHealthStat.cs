using System;
using Unit.GameScene.Units.Creatures.Module.Systems;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    public class CharacterHealthStat : IHealthStat
    {
        private readonly Func<int> _getHealth;
        private readonly Action<int> _setHealth;

        public CharacterHealthStat(CreatureStat<CharacterStat> creatureStat)
        {
            //TODO : 채이환
            _getHealth = () => creatureStat.Current.MaxHp;
            _setHealth = value =>
            {
                var current = creatureStat.Current;
                current.CurrentHp = value;
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