using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Skills.Units;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories.Units.CharacterSkills.Units
{
    public class KnightSkillFactory : SkillFactory<CharacterSkill>
    {
        private readonly CharacterType _characterType;
        private readonly List<string> _characterSkillPresets;
        
        public KnightSkillFactory(CharacterType characterType, List<string> characterSkillPresets)
        {
            _characterType = characterType;
            _characterSkillPresets = characterSkillPresets;
        }

        public override List<CharacterSkill> CreateSkill()
        {
            var skillPresets = new List<CharacterSkill>();
            
            foreach (var characterSkillPreset in _characterSkillPresets)
            {
                var skillType = Enum.Parse<KnightSkillType>(characterSkillPreset);
                
                switch (skillType)
                {
                    case KnightSkillType.LightAttack:
                        skillPresets.Add(new LightAttack(_characterType, skillType));
                        break;
                    case KnightSkillType.SwordBuff:
                        skillPresets.Add(new SwordBuff(_characterType, skillType));
                        break;
                    case KnightSkillType.HolyHeal:
                        skillPresets.Add(new HolyHeal(_characterType, skillType));
                        break;
                    case KnightSkillType.HolySlash:
                        skillPresets.Add(new HolySlash(_characterType, skillType));
                        break;
                }
            }

            return skillPresets;
        }
    }
}