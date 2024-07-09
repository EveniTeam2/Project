using System.Collections.Generic;

namespace Unit.Character {
    public interface IState {
        public void Enter();
        public void Exit();
        public void Update();
        public void FixedUpdate();
        public bool CanTransition();
    }
    public class StateMachine {
        protected Dictionary<string, IState> _states = new Dictionary<string, IState>();
        protected IState _current;
        public StateMachine(string name, IState defaultState) {
            TryAddState(name, defaultState);
            _current = defaultState;
        }
        public virtual bool TryAddState(string name, IState state) {
            if (_states.ContainsKey(name)) {
                return false;
            }
            else {
                _states.Add(name, state);
                return true;
            }
        }
        public virtual bool TryChangeState(string name) {
            if (_states.TryGetValue(name, out var state)) {
                if (_current is StateMachine sub && sub.TryChangeState(name)) {
                    return true;
                }
                else {
                    _current.Exit();
                    _current = state;
                    _current.Enter();
                }
                return true;
            }
            else
                return false;
        }
        public virtual void Update() {
            _current.Update();
        }
        public virtual void FixedUpdate() {
            _current.FixedUpdate();
        }
        public virtual bool CanTransition() {
            return _current.CanTransition();
        }
    }
}