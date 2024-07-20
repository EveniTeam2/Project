using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Stages.Creatures.Interfaces
{
    public interface IState
    {
        event Action OnEnter;
        event Action OnExit;
        event Action OnUpdate;
        event Action OnFixedUpdate;
        void Enter();
        void Exit();
        void Update();
        void FixedUpdate();
        //bool CanTransitionToOther();
        StateType GetStateType();
    }

    public enum EStateEventType
    {
        Enter,
        Exit,
        Update,
        FixedUpdate,
    }
}