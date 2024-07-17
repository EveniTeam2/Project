using Unit.GameScene.Stages.Creatures.Units.Characters;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract
{
    public abstract class CharacterSkill : Skill, ICharacterSkill
    {
        public Character Character { get; private set; }
        public CharacterType CharacterType { get; }

        protected CharacterSkill(CharacterType characterType)
        {
            CharacterType = characterType;
        }

        public void RegisterCharacterReference(Character character)
        {
            Character = character;
        }
        
        public override void ActivateSkill() { }
    }
}