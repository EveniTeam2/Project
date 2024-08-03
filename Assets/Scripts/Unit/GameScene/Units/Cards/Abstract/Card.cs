using Unit.GameScene.Units.Cards.Data;
using UnityEngine;

namespace Unit.GameScene.Units.Cards.Abstract
{
    public abstract class Card
    {
        public string CardName => _cardData.CardName;
        public string CardDescription => _cardData.CardDescription;
        public Sprite CardImage => _cardData.CardImage;
        public int CardLevel => _cardData.CardLevel;
        

        private readonly CardData _cardData;

        protected Card(CardData cardData)
        {
            _cardData = cardData;
        }

        public abstract void ActivateCardEffect();
    }
}