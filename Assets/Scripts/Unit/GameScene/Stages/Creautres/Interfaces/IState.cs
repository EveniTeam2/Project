using System;
using Unit.GameScene.Stages.Creautres.Characters.Enums;
using Unit.GameScene.Stages.Creautres.FSM;

namespace Unit.GameScene.Stages.Creautres.Interfaces
{
    public interface IState
    {
        StateEnums StateName { get; }
        int ParameterHash { get; }
        StateMachine StateMachine { get; }
        event Func<IState, IState> OnEnter;
        event Func<IState, IState> OnExit;
        void Enter(BaseCreature target);
        void Exit(BaseCreature target);
        void Update(BaseCreature target);
        void FixedUpdate(BaseCreature target);
        bool CanTransitionToThis(BaseCreature target);
    }
}