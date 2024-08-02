using System;

namespace Unit.GameScene.Units.Creatures.Interfaces
{
    public interface ITakePlayerDamage
    {
        public void TakeDamage(int value, Action<int> onIncreasePlayerExp);
    }
}