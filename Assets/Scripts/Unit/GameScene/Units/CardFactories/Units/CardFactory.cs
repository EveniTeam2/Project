using System.Collections.Generic;
using Unit.GameScene.Units.Cards.Abstract;

namespace Unit.GameScene.Units.CardFactories.Units
{
    public class CardFactory
    {
        public CardFactory()
        {
            
        }

        public Dictionary<string, Card> CreateCard()
        {
            var cards = new Dictionary<string, Card>();

            return cards;
        }
    }
}