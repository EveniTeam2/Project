using System;
using Unit.GameScene.Stages.Creatures.Units.Monsters.Modules;

namespace Unit.GameScene.Stages.Creatures.Module
{
    public abstract class HealthSystem
    {
        protected IHealthStat _healthStat;
        protected event Action _onHeal;

        public HealthSystem(IHealthStat stats)
        {
            _healthStat = stats;
        }

        protected bool _isDead => _healthStat.GetHealth() <= 0;

        protected int _health
        {
            get => _healthStat.GetHealth();
            set => _healthStat.SetHealth(value);
        }

        protected event Action _onDeath;
        protected event Action _onDamage;

        public abstract void Damage(int dmg);

        public void RegistOnDeathEvent(Action subscriber)
        {
            _onDeath += subscriber;
        }

        public void UnregistOnDeathEvent(Action subscriber)
        {
            _onDeath -= subscriber;
        }

        public void RegistOnDamageEvent(Action subscriber)
        {
            _onDamage += subscriber;
        }

        public void UnregistOnDamageEvent(Action subscriber)
        {
            _onDamage -= subscriber;
        }

        public void ClearEvent()
        {
            _onDeath = null;
            _onDamage = null;
        }

        protected void CallDeath()
        {
            _onDeath?.Invoke();
        }

        protected void CallDamage()
        {
            _onDamage?.Invoke();
        }

        protected void CallHealth()
        {
            _onHeal?.Invoke();
        }

        public void SpawnInit(MonsterHealthStat monsterHealthStat)
        {
            _healthStat = monsterHealthStat;
        }

        public abstract void Heal(int healAmount);
    }

    public interface IHealthStat
    {
        int GetHealth();
        void SetHealth(int health);
    }
}