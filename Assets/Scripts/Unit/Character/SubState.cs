using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Unit.Character {
    public class SubState : StateMachine, IState {
        protected StateMachine _parent;
        protected Dictionary<string, Func<BaseCharacter, bool>> conditions = new Dictionary<string, Func<BaseCharacter, bool>>();
        protected IState _defaultState;

        public SubState(StateMachine sm, string name, IState defaultState, Func<BaseCharacter, bool> condition) : base(sm.Target, name, defaultState) {
            _parent = sm;
            conditions.Add(name, condition);
            _defaultState = defaultState;
        }
        void IState.Enter(BaseCharacter self) {
            foreach (var (name, condition) in conditions) {
                if (condition.Invoke(Target)) {
                    _current = _states[name];
                    _current.Enter(Target);
                    break;
                }
            }
            _current = _defaultState;
            _defaultState.Enter(Target);
        }
        void IState.Exit(BaseCharacter target) {
            _current.Exit(Target);
        }

        public override bool TryAddState(string name, IState state) {
            if (base.TryAddState(name, state)) {
                conditions.Add(name, state.CanTransitionToThis);
                return true;
            }
            else
                return false;
        }

        bool IState.CanTransitionToThis(BaseCharacter target) {
            foreach (var (name, condition) in conditions) {
                if (condition.Invoke(Target)) {
                    return true;
                }
            }
            return false;
        }
    }
}