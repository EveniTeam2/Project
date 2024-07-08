using System;
using System.Collections.Generic;

namespace Unit.Character {
    public class SubState : StateMachine, IState {
        protected Dictionary<string, Func<bool>> conditions = new Dictionary<string, Func<bool>>();
        protected IState _defaultState;

        public SubState(string name, IState defaultState, Func<bool> condition) : base(name, defaultState) {
            conditions.Add(name, condition);
            _defaultState = defaultState;
        }
        public virtual void Enter() {
            foreach (var (name, condition) in conditions) {
                if (condition.Invoke()) {
                    _current = _states[name];
                    _current.Enter();
                    break;
                }
            }
            _current = _defaultState;
            _defaultState.Enter();
        }
        public virtual void Exit() {
            _current.Exit();
        }
    }
}