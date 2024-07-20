using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    public struct StateCommand : IStateCommand
    {
        private StateType _stateType;

        public StateCommand(StateType stateType)
        {
            _stateType = stateType;
        }
        public StateType StateType { get => _stateType; }

        public StateType Execute()
        {
            return StateType;
        }

        public StateType Peek()
        {
            return StateType;
        }
    }
}