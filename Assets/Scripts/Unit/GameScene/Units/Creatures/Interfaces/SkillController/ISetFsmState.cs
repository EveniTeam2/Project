using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillController
{
    public interface ISetFsmState
    {
        void TryChangeState(StateType targetState);
    }
}