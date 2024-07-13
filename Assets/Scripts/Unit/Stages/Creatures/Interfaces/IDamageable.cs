using System;

namespace Unit.Stages.Creatures.Interfaces
{
    public interface IDamageable
    {
        event Action<IDamageable> OnDamage;
        int Health { get; }
        bool IsDead { get; }
        event Action<BaseCreature> OnDeath;
        void Damage(int dmg);
        void SetHealth(int health);
    }
}