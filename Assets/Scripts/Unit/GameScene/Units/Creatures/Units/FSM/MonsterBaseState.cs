using System;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public abstract class MonsterBaseState : IState
    {
        public event Action OnEnter;
        public event Action OnExit;
        public event Action OnUpdate;
        public event Action OnFixedUpdate;

        protected readonly MonsterBaseStateInfo _monsterBaseStateInfo;
        protected Func<StateType, bool> _tryChangeState;
        protected AnimatorEventReceiver animatorEventReceiver;

        protected MonsterBaseState(MonsterBaseStateInfo monsterBaseStateInfo,
                                   Func<StateType, bool> tryChangeState,
                                   AnimatorEventReceiver animatorEventReceiver)
        {
            this._monsterBaseStateInfo = monsterBaseStateInfo;
            _tryChangeState = tryChangeState;
            this.animatorEventReceiver = animatorEventReceiver;
        }

        /// <summary>
        ///     상태에 진입합니다.
        /// </summary>
        public virtual void Enter()
        {
            //Debug.Log($"{_monsterBaseStateInfo.stateType} Enter");
            OnEnter?.Invoke();
        }

        /// <summary>
        ///     상태에서 나옵니다.
        /// </summary>
        public virtual void Exit()
        {
            //Debug.Log($"{_monsterBaseStateInfo.stateType} Out");
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

        protected void ChangeToDefaultState()
        {
            _tryChangeState.Invoke(_monsterBaseStateInfo._defaultExitState);
        }

        public StateType GetStateType()
        {
            return _monsterBaseStateInfo.stateType;
        }
    }
}