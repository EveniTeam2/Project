using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    /// <summary>
    ///     상태 머신을 관리하는 클래스입니다.
    /// </summary>
    public class StateMachine
    {
        private readonly Animator animator;
        protected IState _current;
        protected IState _prev;
        protected Dictionary<StateType, IState> _states;
        protected IState CurrentState => _current;
        protected IState PrevState => _prev;

        public StateMachine(Animator animator)
        {
            _states = new Dictionary<StateType, IState>();
            this.animator = animator;
        }

        /// <summary>
        ///     상태를 추가합니다.
        /// </summary>
        public bool TryAddState(StateType stateType, IState state)
        {
            var result = _states.TryAdd(stateType, state);

            if (_states.Count == 1)
            {
                _current = state;
                _current.Enter();
            }

            return result;
        }

        public bool TryRemoveState(StateType stateType)
        {
            return _states.Remove(stateType);
        }

        public bool TryChangeState(StateType stateType)
        {
            if (_states.ContainsKey(stateType))
            {
                _current.Exit();
                _prev = _current;
                _current = _states[stateType];
                _current.Enter();
                return true;
            }
            return false;
        }

        /// <summary>
        ///     현재 상태를 업데이트합니다.
        /// </summary>
        public void Update()
        {
            _current.Update();
            //CommandAndUpdate();
        }

        /// <summary>
        ///     현재 상태를 고정 업데이트합니다.
        /// </summary>
        public void FixedUpdate()
        {
            _current.FixedUpdate();
        }

        /// <summary>
        ///     상태 머신을 초기화합니다.
        /// </summary>
        public void Clear()
        {
            _current = null;
            _states.Clear();
        }

        public void RegistOnSkillState(Action OnEnter, Action OnExit, Action OnUpdate, Action OnFixedUpdate)
        {
            var skill = _states[StateType.Skill];
            skill.OnEnter += OnEnter;
            skill.OnExit += OnExit;
            skill.OnUpdate += OnUpdate;
            skill.OnFixedUpdate += OnFixedUpdate;
        }

        public StateType GetCurrentStateType()
        {
            return _current.GetStateType();
        }
    }
}