using Unit.GameScene.Stages.Creatures.Units.Characters;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces
{
    public interface ICharacterSkill
    {
        public Character Character { get; }
        public CharacterType CharacterType { get; }
        
        public void RegisterCharacterReference(Character character);
    }
}