using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Enums;
using Unit.GameScene.Units.Cards.Interfaces;

namespace Unit.GameScene.Units.Cards.Units
{
    public abstract class ActiveCard : Card
    {
        public int CardMaxLevel { get; protected set; }
        public int CardCurrentLevel { get; private set; }
        
        protected ActiveCard()
        {
            CardLevelType = CardLevelType.Active;
            CardCurrentLevel = 1;
        }

        protected abstract void UpdateCardData();

        protected bool IncreaseCardLevel()
        {
            if (CardCurrentLevel + 1 > CardMaxLevel)
            {
                CardCurrentLevel = CardMaxLevel;

                return false;
            }

            CardCurrentLevel++;

            UpdateCardData();

            return true;
        }
    }
}