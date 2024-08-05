using System;
using Unit.GameScene.Units.Cards.Data;
using Unit.GameScene.Units.Cards.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Cards.Abstract
{
    public abstract class Card
    {
        public CardTargetType CardTargetType { get; protected set; }
        public CardLevelType CardLevelType { get; protected set; }
        public string CardName { get; protected set; }
        public string CardDescription { get; protected set; }
        public Sprite CardIcon { get; protected set; }
        
        protected Action OnClickCard;

        public abstract bool ActivateCardEffect();
    }
}