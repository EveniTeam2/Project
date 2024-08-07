using System;

namespace Unit.GameScene.Units.Creatures.Interfaces.Battles
{
    public interface ITakePlayerDamage
    {
        Func<int, int> TakeDamage();
    }
}