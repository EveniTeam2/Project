using Unit.GameScene.Units.Creatures.Interfaces.Healths;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public abstract class HealthSystem : ICreatureHealth
    {
        public abstract void TakeHeal(int value);
        public abstract void TakeDamage(int value);
        public abstract void TakeDamageImpact(int value, float duration);
    }
}