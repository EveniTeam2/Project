using System.Collections.Generic;
using Unit.GameScene.Units.Cards.Data;
using Unit.GameScene.Units.Cards.Interfaces;
using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Cards.Units
{
    public class ActiveStatCard : ActiveCard
    {
        private readonly List<StatCardData> _statCardData;
        private readonly IUpdateCreatureStat _character;
        
        private StatType _statType;
        private float _effectValue;
        private float _duration;
        
        public ActiveStatCard(Sprite cardIcon, List<StatCardData> cardData, IUpdateCreatureStat character)
        {
            CardIcon = cardIcon;
            CardMaxLevel = cardData.Count;
            _statCardData = cardData;
            _character = character;
            
            UpdateCardData();
        }

        protected override void UpdateCardData()
        {
            CardName = _statCardData[CardCurrentLevel - 1].CardName;
            CardDescription = _statCardData[CardCurrentLevel - 1].CardDescription;
            _statType = _statCardData[CardCurrentLevel - 1].StatType;
            _effectValue = _statCardData[CardCurrentLevel - 1].CardEffectValue;
            _duration = _statCardData[CardCurrentLevel - 1].Duration;
        }

        public override bool ActivateCardEffect()
        {
            _character.UpdateCreatureStat(_statType, _effectValue);
            
            return IncreaseCardLevel();
        }
    }
}