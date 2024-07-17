using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories
{
    public abstract class SkillFactory<T> where T : Skill
    {
        public abstract List<T> CreateSkill();
    }
}