using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.FSMs.Modules
{
    /// <summary>
    ///     상태 머신을 관리하는 클래스입니다.
    /// </summary>
    public class StateMachine
    {
        private readonly Dictionary<StateType, IState> _states = new();
        
        protected IState CurrentState { get; private set; }
        protected IState PrevState { get; private set; }

        /// <summary>
        ///     상태를 추가합니다.
        /// </summary>
        public bool TryAddState(StateType stateType, IState state)
        {
            bool result = _states.TryAdd(stateType, state);

            if (_states.Count == 1)
            {
                CurrentState = state;
                CurrentState.Enter();
            }

            return result;
        }

        public bool TryRemoveState(StateType stateType)
        {
            return _states.Remove(stateType);
        }

        public bool TryChangeState(StateType stateType)
        {
            if (!_states.TryGetValue(stateType, out _))
                return false;

            CurrentState.Exit();
            PrevState = CurrentState;
            CurrentState = _states[stateType];
            CurrentState.Enter();
            return true;
        }

        /// <summary>
        ///     현재 상태를 업데이트합니다.
        /// </summary>
        public void Update()
        {
            CurrentState.Update();
            //CommandAndUpdate();
        }

        /// <summary>
        ///     현재 상태를 고정 업데이트합니다.
        /// </summary>
        public void FixedUpdate()
        {
            CurrentState.FixedUpdate();
        }

        /// <summary>
        ///     상태 머신을 초기화합니다.
        /// </summary>
        public void Clear()
        {
            CurrentState = null;
            _states.Clear();
        }

        public void RegisterOnSkillState(Action onEnter, Action onExit, Action onUpdate, Action onFixedUpdate)
        {
            IState skill = _states[StateType.Skill];
            skill.OnEnter += onEnter;
            skill.OnExit += onExit;
            skill.OnUpdate += onUpdate;
            skill.OnFixedUpdate += onFixedUpdate;
        }

        public StateType GetCurrentStateType()
        {
            return CurrentState.GetStateType();
        }

        internal void RegisterOnDeathState(Action death)
        {
            Debug.Log("Register on Death");
            _states[StateType.Die].OnExit += death;
        }
    }
}