using System;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules;

namespace Unit.GameScene.Units.Creatures.Module.Systems.Abstract
{
    public abstract class HealthSystem : ICreatureHealth
    {
        protected IHealthStat _healthStat;
        protected bool _invinsible;

        public HealthSystem(IHealthStat stats)
        {
            _healthStat = stats;
            _invinsible = false;
        }

        protected bool _isDead => _healthStat.GetHealth() <= 0;

        protected int _health
        {
            get => _healthStat.GetHealth();
            set => _healthStat.SetHealth(value);
        }

        protected event Action _onHeal;
        protected event Action _onDeath;
        protected event Action _onDamage;

        public bool IsInvinsible()
        {
            return _invinsible;
        }

        public abstract void TakeDamage(int damage);
        public abstract void TakeHeal(int heal);

        public void RegisterOnDeathEvent(Action subscriber)
        {
            _onDeath += subscriber;
        }

        public void UnregisterOnDeathEvent(Action subscriber)
        {
            _onDeath -= subscriber;
        }

        public void RegisterOnDamageEvent(Action subscriber)
        {
            _onDamage += subscriber;
        }

        public void UnregisterOnDamageEvent(Action subscriber)
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
            _invinsible = true;
        }

        protected void CallDamage()
        {
            _onDamage?.Invoke();
        }

        protected void CallHeal()
        {
            _onHeal?.Invoke();
        }

        public void SpawnInit(MonsterHealthStat monsterHealthStat)
        {
            _healthStat = monsterHealthStat;
            _invinsible = false;
        }

    }

    public interface IHealthStat
    {
        int GetHealth();
        void SetHealth(int health);
    }
}