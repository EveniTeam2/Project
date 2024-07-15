using System;
using Unit.GameScene.Stages.Creatures.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Interfaces;

namespace Unit.GameScene.Stages.Creatures.FSM {
    /// <summary>
    /// 상태를 나타내는 기본 클래스입니다.
    /// </summary>
    public class BaseState : IState {
        public event Func<IState, IState> OnEnter;
        public event Func<IState, IState> OnExit;
        protected Func<IState, IState> _onUpdate;
        protected Func<IState, IState> _onFixedUpdate;
        protected Func<BaseCreature, bool> _transitionCondition;
        protected StateMachine _sm;
        protected StateEnums _name;
        protected int _parameterHash;

        public StateEnums StateName => _name;
        public int ParameterHash => _parameterHash;
        public StateMachine StateMachine => _sm;

        public BaseState(StateEnums stateEnums, int aniHash, StateMachine sm, Func<IState, IState> onEnter = null, Func<IState, IState> onExit = null, Func<IState, IState> onUpdate = null, Func<IState, IState> onFixedUpdate = null, Func<BaseCreature, bool> transitionCondition = null) {
            _name = stateEnums;
            _parameterHash = aniHash;
            _sm = sm;
            OnEnter = onEnter;
            OnExit = onExit;
            _onUpdate = onUpdate;
            _onFixedUpdate = onFixedUpdate;
            _transitionCondition = transitionCondition;
        }

        /// <summary>
        /// 상태에 진입합니다.
        /// </summary>
        public void Enter(BaseCreature target) {
            OnEnter?.Invoke(this);
        }

        /// <summary>
        /// 상태에서 나옵니다.
        /// </summary>
        public void Exit(BaseCreature target) {
            OnExit?.Invoke(this);
        }

        /// <summary>
        /// 고정 업데이트를 수행합니다.
        /// </summary>
        public void FixedUpdate(BaseCreature target) {
            _onFixedUpdate?.Invoke(this);
        }

        /// <summary>
        /// 업데이트를 수행합니다.
        /// </summary>
        public void Update(BaseCreature target) {
            _onUpdate?.Invoke(this);
        }

        /// <summary>
        /// 상태 전환이 가능한지 확인합니다.
        /// </summary>
        public bool CanTransitionToThis(BaseCreature target) {
            if (_transitionCondition == null)
                return true;
            return _transitionCondition.Invoke(target);
        }
    }
}