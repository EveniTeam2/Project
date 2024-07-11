using System;

namespace Unit.Character {
    public class BaseState : IState {
        protected Action<BaseState> _onEnter;
        protected Action<BaseState> _onExit;
        protected Action<BaseState> _onUpdate;
        protected Action<BaseState> _onFixedUpdate;
        protected Func<BaseCharacter, bool> _transitionCondition;
        protected StateMachine _sm;
        protected string _name;
        protected int _parameterHash;

        public string StateName => _name;
        public int ParameterHash => _parameterHash;
        public StateMachine StateMachine => _sm;
        public BaseState(string name, int aniHash, StateMachine sm, Action<BaseState> onEnter, Action<BaseState> onExit, Action<BaseState> onUpdate, Action<BaseState> onFixedUpdate, Func<BaseCharacter, bool> transitionCondition) {
            _name = name;
            _parameterHash = aniHash;
            _sm = sm;
            _onEnter = onEnter;
            _onExit = onExit;
            _onUpdate = onUpdate;
            _onFixedUpdate = onFixedUpdate;
            _transitionCondition = transitionCondition;
        }
        public void Enter(BaseCharacter target) {
            _onEnter?.Invoke(this);
            if (_parameterHash != 0) {
                _sm.SetBoolAnimator(_parameterHash, true);
            }
        }
        public void Exit(BaseCharacter target) {
            _onExit?.Invoke(this);
            if (_parameterHash != 0) {
                _sm.SetBoolAnimator(_parameterHash, false);
            }
        }
        public void FixedUpdate(BaseCharacter target) {
            _onFixedUpdate?.Invoke(this);
        }
        public void Update(BaseCharacter target) {
            _onUpdate?.Invoke(this);
        }
        public bool CanTransitionToThis(BaseCharacter target) {
            if (_transitionCondition == null)
                return true;
            return _transitionCondition.Invoke(target);
        }
    }
}