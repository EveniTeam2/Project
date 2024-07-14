using System;
using Unit.GameScene.Stages.Creatures.Interfaces;

namespace Unit.GameScene.Stages.Creatures.FSM {
    /// <summary>
    /// 상태를 나타내는 기본 클래스입니다.
    /// </summary>
    public class BaseState : IState {
        protected Func<IState, IState> _onEnter;
        protected Func<IState, IState> _onExit;
        protected Func<IState, IState> _onUpdate;
        protected Func<IState, IState> _onFixedUpdate;
        protected Func<BaseCreature, bool> _transitionCondition;
        protected StateMachine _sm;
        protected string _name;
        protected int _parameterHash;

        public string StateName => _name;
        public int ParameterHash => _parameterHash;
        public StateMachine StateMachine => _sm;

        public BaseState(string name, int aniHash, StateMachine sm, Func<IState, IState> onEnter = null, Func<IState, IState> onExit = null, Func<IState, IState> onUpdate = null, Func<IState, IState> onFixedUpdate = null, Func<BaseCreature, bool> transitionCondition = null) {
            _name = name;
            _parameterHash = aniHash;
            _sm = sm;
            _onEnter = onEnter;
            _onExit = onExit;
            _onUpdate = onUpdate;
            _onFixedUpdate = onFixedUpdate;
            _transitionCondition = transitionCondition;
        }

        /// <summary>
        /// 상태에 진입합니다.
        /// </summary>
        public void Enter(BaseCreature target) {
            _onEnter?.Invoke(this);
        }

        /// <summary>
        /// 상태에서 나옵니다.
        /// </summary>
        public void Exit(BaseCreature target) {
            _onExit?.Invoke(this);
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