using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Data;
using Unit.GameScene.Units.SkillFactories.Interfaces;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;

namespace Unit.GameScene.Units.Cards.Units
{
    public class SkillCard : Card
    {
        private event Action TryRegisterCharacterSkill;
        
        private readonly SkillCardData _skillCardData;
        
        public SkillCard(SkillCardData cardData) : base(cardData)
        {
            _skillCardData = cardData;
        }
        
        public override void ActivateCardEffect()
        {
            TryRegisterCharacterSkill.Invoke();
            IncreaseSkillLevel();
        }
        
        private void IncreaseSkillLevel()
        {
            _skillCardData.CharacterSkill.OnIncreaseSkillLevel.Invoke();
        }

        public void RegisterHandleOnTryRegisterCharacterSkill(Action action)
        {
            TryRegisterCharacterSkill += action;
        }
    }
}