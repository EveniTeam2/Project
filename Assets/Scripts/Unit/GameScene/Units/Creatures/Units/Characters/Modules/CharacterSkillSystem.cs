using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills.Abstract;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
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

        public void RegisterCharacterServiceProvider(ISkillController skillController)
        {
            foreach (var skill in _skills)
            {
                skill.Value.RegisterCharacterServiceProvider(skillController);
            }
        }

        public CharacterSkill GetDefaultSkill() => (from skill in _skills where skill.Value.GetSkillIndex() == 0 select skill.Value).FirstOrDefault();
        public Sprite GetSkillIcon(string skillName) => _skills[skillName].Icon;
        public int GetSkillValue(string skillName) => _skills[skillName].GetSkillValue();
        public int GetSkillIndex(string skillName) => _skills[skillName].GetSkillIndex();
        public float GetSkillRange1(string skillName) => _skills[skillName].GetSkillRange1();
        public float GetSkillRange2(string skillName) => _skills[skillName].GetSkillRange1();
    }
}