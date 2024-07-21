using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Module;
using UnityEditor;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    /// <summary>
    ///     상태 머신을 관리하는 클래스입니다.
    /// </summary>
    public class StateMachine
    {
        private readonly AnimatorEventReceiver _animatorEventReceiver;
        protected IState _current;
        protected IState _prev;
        protected Dictionary<StateType, IState> _states;
        protected IState CurrentState => _current;
        protected IState PrevState => _prev;
        protected Queue<IStateCommand> _command;

        public StateMachine(AnimatorEventReceiver animatorEventReceiver)
        {
            _states = new Dictionary<StateType, IState>();
            _animatorEventReceiver = animatorEventReceiver;
            _command = new Queue<IStateCommand>();
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

        /// <summary>
        ///     상태를 변경합니다.
        /// </summary>
        //public bool TryChangeState(StateType stateType, Action onExecute)
        //{
        //    if (_states.ContainsKey(stateType))
        //    {
        //        //CommandReceived(new StateCommandChain(stateType, onExecute));
        //        return true;
        //    }
        //    return false;
        //}

        public bool TryChangeState(StateType stateType)
        {
            if (!_states.TryGetValue(stateType, out var targetState)) return false;
            
            _current.Exit();
            _prev = _current;
            _current = targetState;
            _current.Enter();
            return true;
        }

        //private void CommandReceived(IStateCommand command)
        //{
        //    _command.Enqueue(command);
        //}

        //private void CommandAndUpdate()
        //{
        //    if (_command.Count > 0)
        //    {
        //        if (_current.CanTransitionToOther())
        //        {
        //            var com = _command.Dequeue();
        //            Debug.Log($"StateMachine command : {com.Peek()}");
        //            _current.Exit();
        //            _prev = _current;
        //            _current = _states[com.Execute()];
        //            _current.Enter();
        //        }
        //        else
        //            _current.Update();
        //    }
        //    else
        //        _current.Update();
        //}

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
        ///     상태 전환이 가능한지 확인합니다.
        /// </summary>
        //public bool CanTransition()
        //{
        //    return _current.CanTransitionToOther();
        //}

        /// <summary>
        ///     특정 태그의 애니메이션 정규화된 시간을 반환합니다.
        /// </summary>
        //public float GetAnimationNormalizedTime(StateType tag)
        //{
        //    var current = animator.GetCurrentAnimatorStateInfo(0);
        //    var next = animator.GetNextAnimatorStateInfo(0);
        //    if (current.IsTag($"{tag}") || current.IsName($"{tag}"))
        //        return current.normalizedTime;
        //    if (next.IsTag($"{tag}") || next.IsName($"{tag}"))
        //        return next.normalizedTime;
        //    return 0f;
        //}

        /// <summary>
        ///     상태 머신을 초기화합니다.
        /// </summary>
        public void Clear()
        {
            _current = null;
            _states.Clear();
        }

        /// <summary>
        ///     애니메이터의 bool 파라미터를 설정합니다.
        /// </summary>
        //public void SetBoolAnimator(int parameterHash, bool onoff)
        //{
        //    animator.SetBool(parameterHash, onoff);
        //}

        //public bool TryGetState(StateType stateType, out IState state)
        //{
        //    if (_states.TryGetValue(stateType, out state))
        //        return true;
        //    state = null;
        //    return false;
        //}

        public void RegisterOnSkillState(Action onEnter, Action onExit, Action onUpdate, Action onFixedUpdate)
        {
            var skill = _states[StateType.Skill];
            skill.OnEnter += onEnter;
            skill.OnExit += onExit;
            skill.OnUpdate += onUpdate;
            skill.OnFixedUpdate += onFixedUpdate;
        }

        public StateType GetCurrentStateType()
        {
            return _current.GetStateType();
        }
    }
}