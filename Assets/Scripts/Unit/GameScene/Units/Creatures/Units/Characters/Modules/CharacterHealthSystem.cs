using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    public class CharacterHealthSystem : HealthSystem, ICharacterHealth
    {
        public CharacterHealthSystem(CharacterHealthStat stats) : base(stats)
        {
        }

        public override void TakeDamage(int dmg)
        {
            if (_invinsible)
                return;

            _health -= dmg;
            if (_health <= 0)
            {
                _health = 0;
                CallDeath();
            }
            else
                CallDamage();
        }

        public override void TakeHeal(int healAmount)
        {
            _health += healAmount;
            CallHeal();
        }
    }
}