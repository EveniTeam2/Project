using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    public struct StateCommandChain : IStateCommand
    {
        private StateType _stateType;
        private Action onExecute;

        public StateCommandChain(StateType stateType, Action onExecute)
        {
            this.onExecute = onExecute;
            _stateType = stateType;
        }

        public Action OnExecute { get => onExecute; }
        public StateType StateType { get => _stateType; }

        public StateType Execute()
        {
            OnExecute();
            return StateType;
        }

        public StateType Peek()
        {
            return StateType;
        }
    }
}