using System;
using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Blocks.Units.MatchBlock.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Centaurs.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Wizard.Enums;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules
{
    public class SkillManager
    {
        private readonly CharacterClassType _type;
        private readonly Dictionary<string, CharacterSkill> _skills;
        private readonly List<SkillData> _skillData;

        public SkillManager(CharacterClassType type, Dictionary<string, CharacterSkill> skills, List<SkillData> skillData)
        {
            _type = type;
            _skills = skills;
            _skillData = skillData;
        }

        public void RegisterCharacterServiceProvider(ICreatureServiceProvider creatureServiceProvider)
        {
            foreach (var skill in _skills)
            {
                skill.Value.RegisterCharacterServiceProvider(creatureServiceProvider);
            }
        }

        public Sprite GetSkillIcon(string skillName)
        {
            return _skills[skillName].Icon;
        }

        public CharacterSkill GetDefaultSkill()
        {
            return _skillData.Where(skillData => $"{skillData.SkillType}" == $"{BlockType.Default}").Select(skillData => _skills[skillData.SkillTypeEnum]).FirstOrDefault();
        }
        
        public int GetSkillValue(string skillName)
        {
            return (from skillData in _skillData where skillData.SkillName == skillName && skillData.SkillLevel == _skills[skillName].SkillLevel select skillData.SkillEffectValue).FirstOrDefault();
        }

        public float GetSkillRange(string skillName)
        {
            return (from skillData in _skillData where skillData.SkillName == skillName && skillData.SkillLevel == _skills[skillName].SkillLevel select skillData.SkillRange).FirstOrDefault();
        }

        public int GetSkillIndex(string skillName)
        {
            return _type switch
            {
                CharacterClassType.Knight => (int) Enum.Parse<KnightSkillType>(skillName),
                CharacterClassType.Wizard => (int) Enum.Parse<WizardSkillType>(skillName),
                CharacterClassType.Centaurs => (int) Enum.Parse<CentaursSkillType>(skillName)
            };
        }
    }
}