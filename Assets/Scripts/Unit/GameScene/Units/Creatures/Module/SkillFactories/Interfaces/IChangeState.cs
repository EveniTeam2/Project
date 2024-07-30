using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Interfaces
{
    public interface IChangeState
    {
        public void ChangeState(StateType targetState);
    }
}