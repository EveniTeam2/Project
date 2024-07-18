using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.FSM;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract
{
    public abstract class Skill
    {
        public ICreatureServiceProvider ServiceProvider;
        public int SkillAnimationIndex;

        public virtual void RegisterServiceProvider(ICreatureServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}