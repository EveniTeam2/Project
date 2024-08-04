using Unit.GameScene.Units.Cards.Enums;
using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Cards.Data
{
    public struct StatCardData
    {
        public CardType CardType { get; private set; }
        public int CardIndex { get; private set; }
        public StatType StatType { get; private set; }
        public string CardName { get; private set; }
        public string CardDescription { get; private set; }
        public int CardLevel { get; private set; }
        public float CardEffectValue { get; private set; }
        public float Duration { get; private set; }
        
        
        public StatCardData(int cardIndex, CardType cardType, string cardName, string cardDescription, int cardLevel, StatType statType, float cardValue, float duration)
        {
            CardIndex = cardIndex;
            StatType = statType;
            CardEffectValue = cardValue;
            Duration = duration;
            CardType = cardType;
            CardName = cardName;
            CardDescription = cardDescription;
            CardLevel = cardLevel;
        }
    }
}