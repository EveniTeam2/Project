using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills.Abstract;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills
{
    public class CharacterActiveHealSkill : CharacterSkill
    {
        public CharacterActiveHealSkill(List<SkillData> csvData) : base(csvData) { }

        public override void ActivateSkillEffects()
        {
            throw new System.NotImplementedException();
        }
    }
}