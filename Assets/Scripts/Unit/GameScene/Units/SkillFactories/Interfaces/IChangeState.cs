using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.SkillFactories.Interfaces
{
    public interface IChangeState
    {
        public void ChangeState(StateType targetState);
    }
}