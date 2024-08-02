using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Systems
{
    public class MonsterHealthSystem : HealthSystem
    {
        private readonly MonsterStatSystem _monsterStatSystem;
        
        public MonsterHealthSystem(MonsterStatSystem monsterStatSystem)
        {
            _monsterStatSystem = monsterStatSystem;
        }

        public override void TakeHeal(int value)
        {
            _monsterStatSystem.HandleOnUpdateStat(StatType.CurrentHp, value);
        }

        public override void TakeDamage(int value)
        {
            _monsterStatSystem.HandleOnUpdateStat(StatType.CurrentHp, value);
        }
    }
}