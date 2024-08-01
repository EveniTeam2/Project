using System;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;

namespace Unit.GameScene.Units.FSMs.Units.Character.States
{
    /// <summary>
    ///     상태를 나타내는 기본 클래스입니다.
    /// </summary>
    public abstract class CharacterBaseState : IState
    {
        public event Action OnEnter;
        public event Action OnExit;
        public event Action OnUpdate;
        public event Action OnFixedUpdate;

        protected readonly CharacterBaseStateInfo CharacterBaseStateInfo;
        protected readonly Func<StateType, bool> TryChangeState;
        protected readonly AnimatorSystem AnimatorSystem;
        protected readonly ICharacterFsmController FsmController;

        //protected float _enterTime;

        protected CharacterBaseState(CharacterBaseStateInfo characterBaseStateInfo, Func<StateType, bool> tryChangeState, AnimatorSystem animationEventReceiver, ICharacterFsmController fsmController)
        {
            CharacterBaseStateInfo = characterBaseStateInfo;
            TryChangeState = tryChangeState;
            AnimatorSystem = animationEventReceiver;
            FsmController = fsmController;
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
            return CharacterBaseStateInfo.StateType;
        }
    }
}