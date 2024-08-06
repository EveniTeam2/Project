using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems
{
    public class MonsterHealthSystem : HealthSystem
    {
        private readonly MonsterStatSystem _monsterStatSystem;
        private readonly MonsterMovementSystem _movementSystem;

        public MonsterHealthSystem(MonsterStatSystem monsterStatSystem, MonsterMovementSystem movementSystem)
        {
            _monsterStatSystem = monsterStatSystem;
            this._movementSystem = movementSystem;
        }

        public override void TakeHeal(int value)
        {
            _monsterStatSystem.HandleOnUpdateStat(StatType.CurrentHp, value);
        }

        public override void TakeDamage(int value)
        {
            _monsterStatSystem.HandleOnUpdateStat(StatType.CurrentHp, value);
        }

        public override void TakeDamageImpact(int value, float duration = 0.5f)
        {
            TakeDamage(value);
            _movementSystem.SetImpact(duration);
        }
    }
}