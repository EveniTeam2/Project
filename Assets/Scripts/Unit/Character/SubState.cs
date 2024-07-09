using System;
using System.Collections.Generic;

namespace Unit.Character {
    public class SubState : StateMachine, IState {
        protected StateMachine _parent;
        protected Dictionary<string, Func<StateMachine, bool>> conditions = new Dictionary<string, Func<StateMachine, bool>>();
        protected IState _defaultState;

        public SubState(string name, StateMachine sm, IState defaultState, Func<StateMachine, bool> condition) : base(name, defaultState) {
            _parent = sm;
            conditions.Add(name, condition);
            _defaultState = defaultState;
        }
        public virtual void Enter() {
            foreach (var (name, condition) in conditions) {
                if (condition.Invoke(this)) {
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