using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces
{
    public interface IChangeState
    {
        public void ChangeState(StateType targetState);
    }
}