using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;

namespace Unit.GameScene.Stages.Creatures.Interfaces
{
    public interface IState
    {
        StateType StateName { get; }
        int ParameterHash { get; }
        StateMachine StateMachine { get; }
        event Func<IState, IState> OnEnter;
        event Func<IState, IState> OnExit;
        void Enter(Creature target);
        void Exit(Creature target);
        void Update(Creature target);
        void FixedUpdate(Creature target);
        bool CanTransitionToThis(Creature target);
    }
}