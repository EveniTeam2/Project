using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.Healths;

namespace Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems
{
    public class CharacterHealthSystem : HealthSystem, ICharacterHealth
    {
        private readonly CharacterStatSystem _characterStatSystem;
        private readonly CharacterMovementSystem _movementSystem;

        public CharacterHealthSystem(CharacterStatSystem characterStatSystem, CharacterMovementSystem movementSystem)
        {
            _characterStatSystem = characterStatSystem;
            this._movementSystem = movementSystem;
        }

        public override void TakeDamage(int value)
        {
            _characterStatSystem.HandleOnUpdateStat(StatType.CurrentHp, -value);
        }

        public override void TakeHeal(int value)
        {
            _characterStatSystem.HandleOnUpdateStat(StatType.CurrentHp, value);
        }

        public override void TakeDamageImpact(int value, float duration = 0.5f)
        {
            TakeDamage(value);
            _movementSystem.SetImpact(duration);
        }
    }
}