using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Modules;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills.Abstract
{
    public abstract class KnightSkill : CharacterSkill
    {
        protected KnightSkill(KnightSkillData knightSkillData, List<SkillData> csvData) : base(csvData) { }
    }
}