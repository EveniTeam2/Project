using Unit.GameScene.Units.Cards.Enums;

namespace Unit.GameScene.Units.Cards.Abstract
{
    public abstract class PassiveCard : Card
    {
        protected PassiveCard()
        {
            CardLevelType = CardLevelType.Passive;
        }
    }
}