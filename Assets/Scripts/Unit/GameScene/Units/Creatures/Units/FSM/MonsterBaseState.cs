using System;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;

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
        protected readonly MonsterEventPublisher _eventPublisher;

        protected MonsterBaseState(MonsterBaseStateInfo monsterBaseStateInfo,
                                   Func<StateType, bool> tryChangeState,
                                   MonsterEventPublisher eventPublisher)
        {
            this._monsterBaseStateInfo = monsterBaseStateInfo;
            _tryChangeState = tryChangeState;
            this._eventPublisher = eventPublisher;
        }

        /// <summary>
        ///     상태에 진입합니다.
        /// </summary>
        public virtual void Enter()
        {
            //Debug.Log($"{_baseStateInfo.stateType} Enter");
            OnEnter?.Invoke();
            _eventPublisher.RegistOnEvent(eEventType.AnimationEnd, ChangeToDefaultState);
        }

        /// <summary>
        ///     상태에서 나옵니다.
        /// </summary>
        public virtual void Exit()
        {
            //Debug.Log($"{_baseStateInfo.stateType} Out");
            OnExit?.Invoke();
            _eventPublisher.UnregistOnEvent(eEventType.AnimationEnd, ChangeToDefaultState);
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

        private void ChangeToDefaultState()
        {
            _tryChangeState.Invoke(_monsterBaseStateInfo._defaultExitState);
        }

        public StateType GetStateType()
        {
            return _monsterBaseStateInfo.stateType;
        }
    }
}