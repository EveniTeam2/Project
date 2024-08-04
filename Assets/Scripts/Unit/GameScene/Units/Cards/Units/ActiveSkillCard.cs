using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;

namespace Unit.GameScene.Units.Cards.Units
{
    public class ActiveSkillCard : ActiveCard
    {
        private readonly CharacterSkill _characterSkill;

        public ActiveSkillCard(CharacterSkill characterSkill)
        {
            _characterSkill = characterSkill;

            CardIcon = _characterSkill.SkillIcon;
            CardMaxLevel = _characterSkill.SkillMaxLevel;
            
            UpdateCardData();
        }

        protected override void UpdateCardData()
        {
            CardName = _characterSkill.SkillName;
            CardDescription = _characterSkill.GetSkillDescription();
        }
        
        public override bool ActivateCardEffect()
        {
            _characterSkill.InvokeOnIncreaseSkillLevelAction();
            
            return IncreaseCardLevel();
        }
    }
}