using System;

namespace Unit.GameScene.Stages.Creatures.Interfaces
{
    public interface IDamageable
    {
        int Health { get; }
        bool IsDead { get; }
        event Action<BaseCreature> OnDeath;
        void Damage(int dmg);
    }
}