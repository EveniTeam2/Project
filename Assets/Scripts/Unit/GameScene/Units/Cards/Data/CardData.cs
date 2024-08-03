using UnityEngine;

namespace Unit.GameScene.Units.Cards.Data
{
    public class CardData
    {
        public string CardName { get; private set; }
        public string CardDescription{ get; private set; }
        public Sprite CardImage { get; private set; }
        public int CardLevel { get; private set; }

        public CardData(string cardName, string cardDescription, Sprite cardImage, int cardLevel)
        {
            CardName = cardName;
            CardDescription = cardDescription;
            CardImage = cardImage;
            CardLevel = cardLevel;
        }
    }
}