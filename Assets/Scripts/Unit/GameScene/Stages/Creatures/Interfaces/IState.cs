using System;
using Unit.GameScene.Stages.Creatures.Units.Characters;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;

namespace Unit.GameScene.Stages.Creatures.Interfaces
{
    public interface IState
    {
        event Action<StateType, int> OnEnter;
        event Action<StateType, int> OnExit;
        event Action<StateType, int> _onUpdate;
        event Action<StateType, int> _onFixedUpdate;
        void Enter();
        void Exit();
        void Update();
        void FixedUpdate();
        bool CanTransitionToThis();
    }

    public enum EStateEventType {
        Enter,
        Exit,
    }
}