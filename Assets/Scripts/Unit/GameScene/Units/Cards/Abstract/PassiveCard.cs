using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Enums;

namespace Unit.GameScene.Units.Cards.Units
{
    public abstract class PassiveCard : Card
    {
        protected PassiveCard()
        {
            CardLevelType = CardLevelType.Passive;
        }
    }
}