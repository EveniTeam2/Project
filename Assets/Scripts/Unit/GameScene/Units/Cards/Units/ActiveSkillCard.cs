using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Enums;
using Unit.GameScene.Units.Cards.Interfaces;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;

namespace Unit.GameScene.Units.Cards.Units
{
    public class ActiveSkillCard : ActiveCard, IRegisterSkillOnBlock
    {
        public CharacterSkill CharacterSkill { get; private set; }

        public ActiveSkillCard(CharacterSkill characterSkill)
        {
            CardTargetType = CardTargetType.Skill;
            CharacterSkill = characterSkill;
            CardCurrentLevel = CharacterSkill.GetNextLevel();

            CardIcon = CharacterSkill.SkillIcon;
            CardMaxLevel = CharacterSkill.SkillMaxLevel;
            
            UpdateCardData();
        }

        protected override void UpdateCardData()
        {
            CardName = CharacterSkill.SkillName;
            CardDescription = CharacterSkill.GetNextLevelSkillDescription();
        }
        
        public override bool ActivateCardEffect()
        {
            CharacterSkill.InvokeOnIncreaseSkillLevelAction();
            
            return IncreaseCardLevel();
        }

        public int GetSkillIndex()
        {
            return CharacterSkill.SkillIndex;
        }
    }
}