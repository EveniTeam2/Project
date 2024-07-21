using System;
using UnityEngine;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using UnityEngine;
using ScriptableObjects.Scripts.Creature.DTO;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Module;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    /// <summary>
    ///     상태를 나타내는 기본 클래스입니다.
    /// </summary>
    public abstract class BaseState : IState
    {
        public event Action OnEnter;
        public event Action OnExit;
        public event Action OnUpdate;
        public event Action OnFixedUpdate;

        protected readonly BaseStateInfo _baseStateInfo;
        protected Func<StateType, bool> _tryChangeState;
        protected AnimatorEventReceiver _animatorEventReceiver;

        //protected float _enterTime;

        public BaseState(BaseStateInfo baseStateInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animationEventReceiver)
        {
            _baseStateInfo = baseStateInfo;
            _tryChangeState = tryChangeState;
            _animatorEventReceiver = animationEventReceiver;
        }

        /// <summary>
        ///     상태에 진입합니다.
        /// </summary>
        public virtual void Enter()
        {
            //Debug.Log($"{_baseStateInfo.stateType} Enter");
            OnEnter?.Invoke();
            //_enterTime = Time.time;
        }

        /// <summary>
        ///     상태에서 나옵니다.
        /// </summary>
        public virtual void Exit()
        {
            //Debug.Log($"{_baseStateInfo.stateType} Out");
            OnExit?.Invoke();
        }

        /// <summary>
        ///     고정 업데이트를 수행합니다.
        /// </summary>
        public virtual void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        /// <summary>
        ///     업데이트를 수행합니다.
        /// </summary>
        public virtual void Update()
        {
            OnUpdate?.Invoke();
        }

        protected void ClearEvent(EStateEventType type)
        {
            switch (type)
            {
                case EStateEventType.Enter:
                    OnEnter = null;
                    break;
                case EStateEventType.Exit:
                    OnExit = null;
                    break;
                case EStateEventType.Update:
                    OnUpdate = null;
                    break;
                case EStateEventType.FixedUpdate:
                    OnFixedUpdate = null;
                    break;
            }
        }

        public StateType GetStateType()
        {
            return _baseStateInfo.stateType;
        }
    }
}