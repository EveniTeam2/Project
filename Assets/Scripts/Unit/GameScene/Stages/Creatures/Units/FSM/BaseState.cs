using ScriptableObjects.Scripts.Creature.DTO;
using System;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    /// <summary>
    ///     상태를 나타내는 기본 클래스입니다.
    /// </summary>
    public class BaseState : IState
    {
        protected StateType _name;
        protected int _parameterHash;
        public event Action<StateType, int> OnEnter;
        public event Action<StateType, int> OnExit;
        public event Action<StateType, int> _onUpdate;
        public event Action<StateType, int> _onFixedUpdate;
        protected Func<bool> _transitionCondition;

        public BaseState(StateType name, int parameterHash, Action<StateType, int> OnEnter = null, Action<StateType, int> OnExit = null, Action<StateType, int> onUpdate = null, Action<StateType, int> onFixedUpdate = null, Func<bool> transitionCondition = null) {
            _name = name;
            _parameterHash = parameterHash;
            this.OnEnter += OnEnter;
            this.OnExit += OnExit;
            _onUpdate = onUpdate;
            _onFixedUpdate = onFixedUpdate;
            _transitionCondition = transitionCondition;
        }

        /// <summary>
        ///     상태에 진입합니다.
        /// </summary>
        public void Enter()
        {
            OnEnter?.Invoke(_name, _parameterHash);
        }

        /// <summary>
        ///     상태에서 나옵니다.
        /// </summary>
        public void Exit()
        {
            OnExit?.Invoke(_name, _parameterHash);
        }

        /// <summary>
        ///     고정 업데이트를 수행합니다.
        /// </summary>
        public void FixedUpdate()
        {
            _onFixedUpdate?.Invoke(_name, _parameterHash);
        }

        /// <summary>
        ///     업데이트를 수행합니다.
        /// </summary>
        public void Update()
        {
            _onUpdate?.Invoke(_name, _parameterHash);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanTransitionToThis() {
            bool result = _transitionCondition?.Invoke() ?? false;
            return result;
        }
    }
}