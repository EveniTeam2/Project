using System;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Data;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Cards.Units
{
    public class StatCard : Card
    {
        private event Action<StatType, float> OnUpdateCharacterStat;
        private readonly StatCardData _cardData;
        
        public StatCard(StatCardData cardData) : base(cardData)
        {
            _cardData = cardData;
        }

        public override void ActivateCardEffect()
        {
            OnUpdateCharacterStat.Invoke(_cardData.StatType, _cardData.Value);
        }

        public void RegisterHandleOnUpdateCharacterStat(Action<StatType, float> action)
        {
            OnUpdateCharacterStat += action;
        }
    }
}