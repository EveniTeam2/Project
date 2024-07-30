using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Abstract;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories
{
    public abstract class SkillFactory
    {
        public abstract Dictionary<string, CharacterSkill> CreateSkill();
    }
}