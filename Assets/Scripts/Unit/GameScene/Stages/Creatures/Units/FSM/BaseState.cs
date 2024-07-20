using System;
using UnityEngine;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using UnityEngine;
using ScriptableObjects.Scripts.Creature.DTO;
using System.Collections.Generic;

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

        //protected float _enterTime;

        public BaseState(BaseStateInfo baseStateInfo, Func<StateType, bool> tryChangeState)
        {
            _baseStateInfo = baseStateInfo;
            _tryChangeState = tryChangeState;
        }

        /// <summary>
        ///     상태에 진입합니다.
        /// </summary>
        public virtual void Enter()
        {
            Debug.Log($"{_name} Enter");
            OnEnter?.Invoke();
            //_enterTime = Time.time;
        }

        /// <summary>
        ///     상태에서 나옵니다.
        /// </summary>
        public virtual void Exit()
        {
            Debug.Log($"{_name} Out");
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
            //float duration = Time.time - _enterTime;
            //if (duration > _baseStateInfo.animationTime)
            //    _tryChangeState(_baseStateInfo.defaultExitState);
        }

        /// <summary>
        /// </summary>
        //public virtual bool CanTransitionToOther()
        //{
        //    float duration = Time.time - _enterTime;
        //    return duration > _baseStateInfo.canTransitTime;
        //}

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