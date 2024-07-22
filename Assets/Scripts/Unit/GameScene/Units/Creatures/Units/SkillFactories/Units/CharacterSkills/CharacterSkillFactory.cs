using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Units.CharacterSkills.Units;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories.Units.CharacterSkills
{
    public class CharacterSkillFactory : SkillFactory<CharacterSkill>
    {
        private readonly CharacterServiceProvider _characterServiceProvider;
        private readonly List<string> _characterSkillPresets;
        private readonly CharacterType _characterType;

        public CharacterSkillFactory(CharacterServiceProvider characterServiceProvider, CharacterType characterType, List<string> characterSkillPresets)
        {
            _characterType = characterType;
            _characterSkillPresets = characterSkillPresets;
            _characterServiceProvider = characterServiceProvider;
        }

        public override List<CommandAction> CreateSkill()
        {
            return _characterType switch
            {
                CharacterType.Knight => new KnightSkillFactory(_characterSkillPresets, _characterServiceProvider).CreateSkill(),
                _ => null
            };
        }
    }
}