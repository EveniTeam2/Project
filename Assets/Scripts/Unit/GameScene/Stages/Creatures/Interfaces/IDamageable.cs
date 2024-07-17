using System;

namespace Unit.GameScene.Stages.Creatures.Interfaces
{
    public interface IDamageable
    {
        int Health { get; }
        bool IsDead { get; }
        event Action<Creature> OnDeath;
        event Action<Creature> OnDamage;
        void Damage(int dmg);
    }
}