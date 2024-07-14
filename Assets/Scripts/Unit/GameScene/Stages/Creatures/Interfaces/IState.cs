using System;
using Unit.Stages.Creatures.FSM;

namespace Unit.GameScene.Stages.Creatures.Interfaces
{
    public interface IState
    {
        event Func<IState, IState> OnEnter;
        event Func<IState, IState> OnExit;
        void Enter(BaseCreature target);
        void Exit(BaseCreature target);
        void Update(BaseCreature target);
        void FixedUpdate(BaseCreature target);
        bool CanTransitionToThis(BaseCreature target);
        string StateName { get; }
        int ParameterHash { get; }
        StateMachine StateMachine { get; }
    }
}
