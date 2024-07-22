using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories
{
    public abstract class SkillFactory<T> where T : Skill
    {
        public abstract List<CommandAction> CreateSkill();
    }
}