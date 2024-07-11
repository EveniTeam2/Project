using System;

namespace Unit.Character {
    public class BaseState : IState {
        protected Action _onEnter;
        protected Action _onExit;
        protected Action _onUpdate;
        protected Action _onFixedUpdate;
        protected Func<BaseCharacter, bool> _transitionCondition;
        protected StateMachine _sm;
        protected string _name;
        protected int _parameterHash;

        public string StateName => _name;

        public BaseState(string name, int aniHash, StateMachine sm, Action onEnter, Action onExit, Action onUpdate, Action onFixedUpdate, Func<BaseCharacter, bool> transitionCondition) {
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
            _onEnter?.Invoke();
            if (_parameterHash != 0) {
                _sm.SetBoolAnimator(_parameterHash, true);
            }
        }
        public void Exit(BaseCharacter target) {
            _onExit?.Invoke();
            if (_parameterHash != 0) {
                _sm.SetBoolAnimator(_parameterHash, false);
            }
        }
        public void FixedUpdate(BaseCharacter target) {
            _onFixedUpdate?.Invoke();
        }
        public void Update(BaseCharacter target) {
            _onUpdate?.Invoke();
        }
        public bool CanTransitionToThis(BaseCharacter target) {
            if (_transitionCondition == null)
                return true;
            return _transitionCondition.Invoke(target);
        }
    }
}