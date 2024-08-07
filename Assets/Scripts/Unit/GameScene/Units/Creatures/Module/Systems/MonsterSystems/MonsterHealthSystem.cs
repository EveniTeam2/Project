using System;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems
{
    public class MonsterHealthSystem : HealthSystem
    {
        private readonly MonsterMovementSystem _monsterMovementSystem;
        private readonly Action<StatType, float> _onUpdatePermanentStat;
        private readonly Action<StatType, float, float> _onUpdateTemporaryStat;
        
        public MonsterHealthSystem(MonsterMovementSystem monsterMovementSystem, Action<StatType, float> onUpdatePermanentStat, Action<StatType, float, float> onUpdateTemporaryStat)
        {
            _monsterMovementSystem = monsterMovementSystem;
            _onUpdatePermanentStat = onUpdatePermanentStat;
            _onUpdateTemporaryStat = onUpdateTemporaryStat;
        }

        public override void TakeHeal(int value)
        {
            _onUpdatePermanentStat.Invoke(StatType.CurrentHp, value);
        }

        public override void TakeDamage(int value)
        {
            _monsterMovementSystem.SetImpact();
            _onUpdatePermanentStat.Invoke(StatType.CurrentHp, value);
        }
    }
}