using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories.Units.CharacterSkills.Units
{
    public class KnightSkillFactory : SkillFactory<CharacterSkill>
    {
        private readonly List<string> _characterSkillPresets;
        private readonly CharacterServiceProvider _characterServiceProvider;

        public KnightSkillFactory(List<string> characterSkillPresets, CharacterServiceProvider characterServiceProvider)
        {
            _characterSkillPresets = characterSkillPresets;
            _characterServiceProvider = characterServiceProvider;
        }

        public override List<CommandAction> CreateSkill()
        {
            var skillPresets = new List<CommandAction>();

            foreach (var characterSkillPreset in _characterSkillPresets)
            {
                var skillType = Enum.Parse<KnightSkillType>(characterSkillPreset);
                Debug.Log($"string '{characterSkillPreset}' => {skillType} 파싱");

                switch (skillType)
                {
                    case KnightSkillType.BaseAttack:
                        ISkillCommand knightBaseAttack = new KnightBaseAttack(_characterServiceProvider);
                        skillPresets.Add(new CommandAction(knightBaseAttack));
                        break;
                    case KnightSkillType.SwordBuff:
                        ISkillCommand knightSwordBuff = new KnightSwordBuff(_characterServiceProvider);
                        skillPresets.Add(new CommandAction(knightSwordBuff));
                        break;
                    case KnightSkillType.HolyHeal:
                        ISkillCommand knightHolyHeal = new KnightHolyHeal(_characterServiceProvider);
                        skillPresets.Add(new CommandAction(knightHolyHeal));
                        break;
                    case KnightSkillType.HolySlash:
                        ISkillCommand knightHolySlash = new KnightHolySlash(_characterServiceProvider);
                        skillPresets.Add(new CommandAction(knightHolySlash));
                        break;
                }
            }

            return skillPresets;
        }
    }
}