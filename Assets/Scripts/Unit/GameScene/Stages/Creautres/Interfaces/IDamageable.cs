using System;

namespace Unit.GameScene.Stages.Creautres.Interfaces
{
    public interface IDamageable
    {
        int Health { get; }
        bool IsDead { get; }
        event Action<BaseCreature> OnDeath;
        event Action<BaseCreature> OnDamage;
        void Damage(int dmg);
    }
}