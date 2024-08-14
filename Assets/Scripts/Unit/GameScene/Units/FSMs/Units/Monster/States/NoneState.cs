using System;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.FSMs.Units.Monster.Structs;

namespace Unit.GameScene.Units.FSMs.Units.Monster.States
{
    public class NoneState : IState
    {
        public event Action OnEnter;
        public event Action OnExit;
        public event Action OnUpdate;
        public event Action OnFixedUpdate;

        public void Enter()
        {
            OnEnter?.Invoke();
        }

        public void Exit()
        {
            OnExit?.Invoke();
        }

        public void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        public StateType GetStateType()
        {
            return StateType.None;
        }

        public void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}