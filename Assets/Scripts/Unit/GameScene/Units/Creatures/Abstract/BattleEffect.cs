using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public class BattleEffect : IBattleEffect
    {
        private int damage;

        public BattleEffect(int damage)
        {
            this.damage = damage;
        }

        public void Attack<TargetType>(TargetType target) where TargetType : Creature
        {
            target.OnUpdateStat?.Invoke(StatType.CurrentHp, damage);
        }
    }
}