using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;

namespace Unit.GameScene.Stages.Creatures.FSM
{
    /// <summary>
    /// 상태 머신을 관리하는 클래스입니다.
    /// </summary>
    public class StateMachine
    {
        protected Dictionary<string, IState> _states = new Dictionary<string, IState>();
        protected IState _current;
        protected IState _prev;
        public BaseCreature Target { get; protected set; }
        public IState CurrentState => _current;
        public IState PrevState => _prev;

        public StateMachine(BaseCreature creature)
        {
            Target = creature;
        }

        /// <summary>
        /// 상태를 추가합니다.
        /// </summary>
        public bool TryAddState(string name, IState state)
        {
            if (_current == null)
            {
                _current = state;
                _current.Enter(Target);
            }

            if (_states.ContainsKey(name))
            {
                return false;
            }
            else
            {
                _states.Add(name, state);
                return true;
            }
        }

        /// <summary>
        /// 상태를 변경합니다.
        /// </summary>
        public bool TryChangeState(string name)
        {
            if (_states.TryGetValue(name, out var state))
            {
                _current.Exit(Target);
                _prev = _current;
                _current = state;
                _current.Enter(Target);
                return true;
            }
            //else
            //{
            //    foreach (var item in _states.Values)
            //    {
            //        if (item is StateMachine sub && sub.TryChangeState(name))
            //        {
            //            return true;
            //        }
            //    }
            //}
            return false;
        }

        /// <summary>
        /// 현재 상태를 업데이트합니다.
        /// </summary>
        public void Update(BaseCreature target)
        {
            _current.Update(Target);
        }

        /// <summary>
        /// 현재 상태를 고정 업데이트합니다.
        /// </summary>
        public void FixedUpdate(BaseCreature target)
        {
            _current.FixedUpdate(Target);
        }

        /// <summary>
        /// 상태 전환이 가능한지 확인합니다.
        /// </summary>
        public bool CanTransition()
        {
            return _current.CanTransitionToThis(Target);
        }

        /// <summary>
        /// 현재 애니메이션의 정규화된 시간을 반환합니다.
        /// </summary>
        public float GetCurrentAnimationNormalizedTime()
        {
            return GetAnimationNormalizedTime(_current.StateName);
        }

        /// <summary>
        /// 특정 태그의 애니메이션 정규화된 시간을 반환합니다.
        /// </summary>
        public float GetAnimationNormalizedTime(string tag)
        {
            var current = Target.Animator.GetCurrentAnimatorStateInfo(0);
            var next = Target.Animator.GetNextAnimatorStateInfo(0);
            if (current.IsTag(tag) || current.IsName(tag))
                return current.normalizedTime;
            else if (next.IsTag(tag) || next.IsName(tag))
                return next.normalizedTime;
            return 0f;
        }

        /// <summary>
        /// 상태 머신을 초기화합니다.
        /// </summary>
        public void Clear()
        {
            _current = null;
            _states.Clear();
        }

        /// <summary>
        /// 애니메이터의 bool 파라미터를 설정합니다.
        /// </summary>
        public void SetBoolAnimator(int parameterHash, bool onoff)
        {
            Target.Animator.SetBool(parameterHash, onoff);
        }
    }
}