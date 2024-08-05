using System;
using Unit.GameScene.Units.Cards.Data;
using Unit.GameScene.Units.Cards.Enums;
using Unit.GameScene.Units.Cards.Interfaces;
using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Cards.Units
{
    public class PassiveStatCard : PassiveCard
    {
        private readonly StatCardData _statCardData;
        private readonly IUpdateCreatureStat _character;
        
        public PassiveStatCard(Sprite cardIcon, StatCardData cardData, IUpdateCreatureStat character)
        {
            CardTargetType = CardTargetType.Stat;
            
            CardIcon = cardIcon;

            _statCardData = cardData;
            CardName = _statCardData.CardName;
            CardDescription = _statCardData.CardDescription;
            _character = character;
        }

        public override bool ActivateCardEffect()
        {
            _character.UpdateCreatureStat(_statCardData.StatType, _statCardData.CardEffectValue);

            return true;
        }
    }
}