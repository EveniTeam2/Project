using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills.Abstract;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills
{
    public class CharacterActiveAttackSkill : CharacterSkill
    {
        public CharacterActiveAttackSkill(List<SkillData> csvData) : base(csvData) { }

        public override void ActivateSkillEffects()
        {
            AttackEnemy(GetSkillEffectValue() * ComboCount, GetSkillRange());
        }
    }
}