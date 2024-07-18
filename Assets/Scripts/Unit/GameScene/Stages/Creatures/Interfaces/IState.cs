using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Stages.Creatures.Interfaces
{
    public interface IState
    {
        event Action<StateType, int> OnEnter;
        event Action<StateType, int> OnExit;
        void Enter();
        void Exit();
        void Update();
        void FixedUpdate();
        bool CanTransitionToThis();
    }

    public enum EStateEventType
    {
        Enter,
        Exit
    }
}