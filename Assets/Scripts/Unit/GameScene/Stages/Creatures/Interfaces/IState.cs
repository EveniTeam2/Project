using System;
using Unit.GameScene.Stages.Creatures.Characters.Enums;
using Unit.GameScene.Stages.Creatures.FSM;

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
        StateEnums StateName { get; }
        int ParameterHash { get; }
        StateMachine StateMachine { get; }
    }
}
