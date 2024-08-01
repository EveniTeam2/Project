using System;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Units.Monster.Structs;
using UnityEngine;

namespace Unit.GameScene.Units.FSMs.Units.Monster.States
{
    public abstract class MonsterBaseState : IState
    {
        public event Action OnEnter;
        public event Action OnExit;
        public event Action OnUpdate;
        public event Action OnFixedUpdate;

        protected readonly MonsterBaseStateInfo MonsterBaseStateInfo;
        protected readonly Func<StateType, bool> TryChangeState;
        protected readonly IMonsterFsmController FsmController;

        protected MonsterBaseState(MonsterBaseStateInfo monsterBaseStateInfo, Func<StateType, bool> tryChangeState, IMonsterFsmController fsmController)
        {
            MonsterBaseStateInfo = monsterBaseStateInfo;
            TryChangeState = tryChangeState;
            FsmController = fsmController;
        }

        /// <summary>
        ///     상태에 진입합니다.
        /// </summary>
        public virtual void Enter()
        {
            Debug.Log($"{MonsterBaseStateInfo.StateType} Enter");
            OnEnter?.Invoke();
        }

        /// <summary>
        ///     상태에서 나옵니다.
        /// </summary>
        public virtual void Exit()
        {
            Debug.Log($"{MonsterBaseStateInfo.StateType} Out");
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
            TryChangeState.Invoke(MonsterBaseStateInfo.DefaultExitState);
        }

        public StateType GetStateType()
        {
            return MonsterBaseStateInfo.StateType;
        }
    }
}