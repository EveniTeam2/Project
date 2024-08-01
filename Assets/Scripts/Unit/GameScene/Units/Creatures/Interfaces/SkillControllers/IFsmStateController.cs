using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillControllers
{
    public interface IFsmStateController
    {
        void TryChangeState(StateType targetState);
    }
}