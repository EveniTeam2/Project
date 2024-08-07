using Unit.GameScene.Units.Cards.Enums;

namespace Unit.GameScene.Units.Cards.Abstract
{
    public abstract class ActiveCard : Card
    {
        public int CardMaxLevel { get; protected set; }
        public int CardCurrentLevel { get; protected set; }
        
        protected ActiveCard()
        {
            CardLevelType = CardLevelType.Active;
        }

        protected abstract void UpdateCardData();

        protected bool IncreaseCardLevel()
        {
            CardCurrentLevel++;
            
            if (CardCurrentLevel + 1 >= CardMaxLevel)
            {
                CardCurrentLevel = CardMaxLevel;
                return false;
            }

            UpdateCardData();

            return true;
        }
    }
}