using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    public interface IStateCommand
    {
        StateType Peek();
        StateType Execute();
    }
}