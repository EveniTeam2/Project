using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Skills.Units;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories.Units.CharacterSkills.Units
{
    public class KnightSkillFactory : SkillFactory<CharacterSkill>
    {
        private readonly List<string> _characterSkillPresets;

        public KnightSkillFactory(List<string> characterSkillPresets)
        {
            _characterSkillPresets = characterSkillPresets;
        }

        public override List<CharacterSkill> CreateSkill()
        {
            var skillPresets = new List<CharacterSkill>();

            foreach (var characterSkillPreset in _characterSkillPresets)
            {
                var skillType = Enum.Parse<KnightSkillType>(characterSkillPreset);
                Debug.Log($"string '{characterSkillPreset}' => {skillType} 파싱");

                switch (skillType)
                {
                    case KnightSkillType.BaseAttack:
                        skillPresets.Add(new KnightBaseAttack());
                        break;
                    // case KnightSkillType.LightAttack:
                    //     skillPresets.Add(new KnightLightAttack());
                    //     break;
                    case KnightSkillType.SwordBuff:
                        skillPresets.Add(new KnightSwordBuff());
                        break;
                    case KnightSkillType.HolyHeal:
                        skillPresets.Add(new KnightHolyHeal());
                        break;
                    case KnightSkillType.HolySlash:
                        skillPresets.Add(new KnightHolySlash());
                        break;
                }
            }

            return skillPresets;
        }
    }
}