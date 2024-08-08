using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Cards.Interfaces
{
    public interface ICardController
    {
        public void UpdateCreatureStat(StatType statType, float value);
    }
}