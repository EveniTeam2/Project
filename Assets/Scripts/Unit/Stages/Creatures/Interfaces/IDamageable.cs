using System;

namespace Unit.Stages.Creatures.Interfaces
{
    public interface IDamageable
    {
        int Health { get; }
        bool IsDead { get; }
        event Action<BaseCreature> OnDeath;
        void Damage(int dmg);
    }
}