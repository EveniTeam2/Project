using System;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.Healths;

namespace Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems
{
    public class CharacterHealthSystem : HealthSystem, ICharacterHealth
    {
        private readonly Action<StatType, float> _onUpdatePermanentStat;
        private readonly Action<StatType, float, float> _onUpdateTemporaryStat;
        
        private readonly CharacterMovementSystem _movementSystem;

        public CharacterHealthSystem(CharacterMovementSystem movementSystem, Action<StatType, float> onUpdatePermanentStat, Action<StatType, float, float> onUpdateTemporaryStat)
        {
            _movementSystem = movementSystem;
            _onUpdatePermanentStat = onUpdatePermanentStat;
            _onUpdateTemporaryStat = onUpdateTemporaryStat;
        }

        public override void TakeDamage(int value)
        {
            _movementSystem.SetImpact();
            _onUpdatePermanentStat.Invoke(StatType.CurrentHp, -value);
        }

        public override void TakeHeal(int value)
        {
            _onUpdatePermanentStat.Invoke(StatType.CurrentHp, value);
        }
    }
}