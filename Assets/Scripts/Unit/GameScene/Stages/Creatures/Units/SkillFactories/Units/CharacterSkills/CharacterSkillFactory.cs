using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Units.CharacterSkills.Units;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories.Units.CharacterSkills
{
    public class CharacterSkillFactory : SkillFactory<CharacterSkill>
    {
        private readonly CharacterType _characterType;
        private readonly List<string> _characterSkillPresets;
        
        public CharacterSkillFactory(CharacterType characterType, List<string> characterSkillPresets)
        {
            _characterType = characterType;
            _characterSkillPresets = characterSkillPresets;
        }
        
        public override List<CharacterSkill> CreateSkill()
        {
            return _characterType switch
            {
                CharacterType.Knight => new KnightSkillFactory(_characterType, _characterSkillPresets).CreateSkill(),
                _ => null
            };
        }
    }
}