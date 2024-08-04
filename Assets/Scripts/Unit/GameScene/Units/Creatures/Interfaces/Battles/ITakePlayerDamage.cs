using System;

namespace Unit.GameScene.Units.Creatures.Interfaces
{
    public interface ITakePlayerDamage
    {
        public Func<int, int> TakeDamage();
    }
}