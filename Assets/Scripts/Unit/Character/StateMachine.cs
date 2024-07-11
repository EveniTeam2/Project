using System;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

namespace Unit.Character {
    public interface IState {
        public void Enter(BaseCharacter target);
        public void Exit(BaseCharacter target);
        public void Update(BaseCharacter target);
        public void FixedUpdate(BaseCharacter target);
        public bool CanTransitionToThis(BaseCharacter target);
        public string StateName { get; }
        public int ParameterHash { get; }
        public StateMachine StateMachine { get; }
    }
    public class StateMachine {
        protected Dictionary<string, IState> _states = new Dictionary<string, IState>();
        protected IState _current;
        public BaseCharacter Target { get; protected set; }
        public StateMachine(BaseCharacter character) {
            Target = character;
        }
        public virtual bool TryAddState(string name, IState state) {
            if (_current == null) {
                _current = state;
                _current.Enter(Target);
            }

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
                _current.Exit(Target);
                _current = state;
                _current.Enter(Target);
                return true;
            }
            else {
                foreach (var item in _states.Values) {
                    if (item is StateMachine sub && sub.TryChangeState(name)) {
                        return true;
                    }
                }
            }
            return false;
        }
        public virtual void Update(BaseCharacter target) {
            _current.Update(Target);
        }
        public virtual void FixedUpdate(BaseCharacter target) {
            _current.FixedUpdate(Target);
        }
        public virtual bool CanTransition() {
            return _current.CanTransitionToThis(Target);
        }

        public virtual float GetCurrentAnimationNormalizedTime() {
            return GetAnimationNormalizedTime(_current.StateName);
        }

        public virtual float GetAnimationNormalizedTime(string tag) {
            var current = Target.Animator.GetCurrentAnimatorStateInfo(0);
            var next = Target.Animator.GetNextAnimatorStateInfo(0);
            if (current.IsTag(tag) || current.IsName(tag))
                return current.normalizedTime;
            else if (next.IsTag(tag) || next.IsName(tag))
                return next.normalizedTime;
            return 0f;
        }

        public virtual void Clear() {
            _current = null;
            _states.Clear();
        }

        public void SetBoolAnimator(int parameterHash, bool onoff) {
            Target.Animator.SetBool(parameterHash, onoff);
        }
    }
}