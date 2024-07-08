using System;

namespace Unit.Character {
    public class BaseState : IState {
        protected Action _onEnter;
        protected Action _onExit;
        protected Action _onUpdate;
        protected Action _onFixedUpdate;
        protected StateMachine _sm;
        protected string _name;
        public string StateName => _name;
        public BaseState(string name, StateMachine sm, Action onEnter, Action onExit, Action onUpdate, Action onFixedUpdate) {
            _name = name;
            _sm = sm;
            _onEnter = onEnter;
            _onExit = onExit;
            _onUpdate = onUpdate;
            _onFixedUpdate = onFixedUpdate;
        }
        public void Enter() {
            _onEnter?.Invoke();
        }
        public void Exit() {
            _onExit?.Invoke();
        }
        public void FixedUpdate() {
            _onFixedUpdate?.Invoke();
        }
        public void Update() {
            _onUpdate?.Invoke();
        }
    }
}