using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.Healths;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules.Systems
{
    public class CharacterHealthSystem : HealthSystem, ICharacterHealth
    {
        private readonly CharacterStatSystem _characterStatSystem;
        
        public CharacterHealthSystem(CharacterStatSystem characterStatSystem) : base()
        {
            _characterStatSystem = characterStatSystem;
        }

        public override void TakeDamage(int value)
        {
            _characterStatSystem.HandleUpdateStat(StatType.CurrentHp, -value);
        }

        public override void TakeHeal(int value)
        {
            _characterStatSystem.HandleUpdateStat(StatType.CurrentHp, value);
        }
        
        public bool IsInvincible()
        {
            throw new System.NotImplementedException();
        }
    }
}