using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems
{
    public class CharacterSkillSystem
    {
        private readonly CharacterClassType _type;
        private readonly Dictionary<string, CharacterSkill> _skills;

        public CharacterSkillSystem(CharacterClassType type, Dictionary<string, CharacterSkill> skills)
        {
            _type = type;
            _skills = skills;
        }

        public void RegisterCharacterServiceProvider(ICharacterSkillController characterSkillController)
        {
            foreach (var skill in _skills)
            {
                skill.Value.RegisterCharacterServiceProvider(characterSkillController);
            }
        }

        public CharacterSkill GetDefaultSkill() => (from skill in _skills where skill.Value.GetSkillIndex() == 0 select skill.Value).FirstOrDefault();
        public Sprite GetSkillIcon(string skillName) => _skills[skillName].SkillIcon;
        public int GetSkillValue(string skillName) => _skills[skillName].GetSkillValue();
        public int GetSkillIndex(string skillName) => _skills[skillName].GetSkillIndex();
        public float GetSkillRange1(string skillName) => _skills[skillName].GetSkillRange1();
        public float GetSkillRange2(string skillName) => _skills[skillName].GetSkillRange1();
    }
}