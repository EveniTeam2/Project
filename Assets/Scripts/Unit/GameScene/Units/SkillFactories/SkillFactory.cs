using System.Collections.Generic;
using Unit.GameScene.Units.SkillFactories.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;

namespace Unit.GameScene.Units.SkillFactories
{
    public abstract class SkillFactory
    {
        public abstract Dictionary<string, CharacterSkill> CreateSkill(List<SkillData> skillCsvData);
    }
}