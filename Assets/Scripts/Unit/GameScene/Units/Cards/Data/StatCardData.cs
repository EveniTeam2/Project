using System;
using Unit.GameScene.Units.CardFactories.Modules;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Cards.Units
{
    public class StatCardData : CardData
    {
        public StatType StatType { get; private set; }
        public float Value { get; private set; }
        
        
        public StatCardData(string cardName, string cardDescription, Sprite cardImage, int cardLevel, StatType statType, float value) : base(cardName, cardDescription, cardImage, cardLevel)
        {
            StatType = statType;
            Value = value;
        }
    }
}