using System;
using System.Collections;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using UnityEditor;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorEventReceiver : MonoBehaviour
    {
        #region Inspector

        public List<AnimationClip> animationClips = new();

        #endregion

        private Animator _animator = null;
        private readonly Dictionary<string, List<Action>> _startEvents = new();
        private readonly Dictionary<string, List<Action>> _endEvents = new();

        private Coroutine _coroutine = null;
        private bool _isPlayingAnimator = false;
        public event Action OnAttack;

        private void Awake()
        {
            // 애니메이터 내에 있는 모든 애니메이션 클립의 시작과 끝에 이벤트를 생성한다.
            _animator = GetComponent<Animator>();
            
            AddEventsToSpecificAnimationClips();
        }

        private void AddEventsToSpecificAnimationClips()
        {
            foreach (var animationClip in animationClips)
            {
                if (TryGetAnimationEvents(animationClip, out var animationEvents))
                {
                    CheckAnimationEventCorrectly(animationClip, animationEvents);
                }
                else
                {
                    var animationStartEvent = new AnimationEvent
                    {
                        time = 0,
                        functionName = "AnimationStartHandler",
                        stringParameter = animationClip.name
                    };

                    animationClip.AddEvent(animationStartEvent);

                    var animationEndEvent = new AnimationEvent
                    {
                        time = animationClip.length-0.1f,
                        functionName = "AnimationEndHandler",
                        stringParameter = animationClip.name
                    };

                    animationClip.AddEvent(animationEndEvent);
                }
            }
        }

        private void AddEventsToAllAnimationClips()
        {
            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
            {
                animationClips.Add(clip);

                var animationStartEvent = new AnimationEvent
                {
                    time = 0,
                    functionName = "AnimationStartHandler",
                    stringParameter = clip.name
                };
                
                clip.AddEvent(animationStartEvent);

                var animationEndEvent = new AnimationEvent
                {
                    time = clip.length,
                    functionName = "AnimationEndHandler",
                    stringParameter = clip.name
                };
                
                clip.AddEvent(animationEndEvent);
            }
        }

        public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layer)
        {
            return _animator.GetCurrentAnimatorStateInfo(layer);
        }

        public AnimatorStateInfo GetNextAnimatorStateInfo(int layer)
        {
            return  _animator.GetNextAnimatorStateInfo(layer);
        }
        
        public void SetTrigger(int param, Action action)
        {
            _animator.SetTrigger(param, action);
        }
        
        public void SetTrigger(string param, Action action)
        {
            _animator.SetTrigger(param, action);
        }
        
        public void SetBool(int param, bool value, Action action)
        {
            _animator.SetBool(param, value, action);
        }

        public void SetBool(string param, bool value, Action action)
        {
            _animator.SetBool(param, value, action);
        }
        
        public void SetFloat(int param, float value, Action action)
        {
            _animator.SetFloat(param, value, action);
        }

        public void SetFloat(string param, float value, Action action)
        {
            _animator.SetFloat(param, value, action);
        }
        
        public void SetInteger(string param, int value, Action action)
        {
            _animator.SetInteger(param, value, action);
        }
        
        public void SetInteger(int param, int value, Action action)
        {
            _animator.SetInteger(param, value, action);
        }

        public void OnStateEnd(Action onFinished)
        {
            if (gameObject.activeInHierarchy == false || onFinished == null) return;
            
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            
            _coroutine = StartCoroutine(OnStateEndCheck(onFinished));
        }

        private IEnumerator OnStateEndCheck(Action onFinished)
        {
            _isPlayingAnimator = true;
            while (true)
            {
                yield return new WaitForEndOfFrame();
                
                if (!_isPlayingAnimator)
                {
                    // 다음 애니메이션 클립이 재생되는지 1프레임 더 기다림
                    yield return new WaitForEndOfFrame();
                    
                    if (!_isPlayingAnimator) break;
                }
            }
            
            onFinished?.Invoke();
        }

        /// <summary>
        /// 각 클립 별 시작 이벤트
        /// </summary>
        /// <param name="name"></param>
        private void AnimationStartHandler(string name)
        {
            if (_startEvents.TryGetValue(name, out var actions))
            {
                foreach (var action in actions)
                {
                    action?.Invoke();
                }

                actions.Clear();
            }
            
            _isPlayingAnimator = true;
        }

        /// <summary>
        /// 클립 별 종료 이벤트
        /// </summary>
        /// <param name="name"></param>
        private void AnimationEndHandler(string name)
        {
            if (_endEvents.TryGetValue(name, out var actions))
            {
                foreach (var action in actions)
                {
                    action?.Invoke();
                }

                actions.Clear();
            }
            
            _isPlayingAnimator = false;
        }

        public void ActivateSkillEffects()
        {
            OnAttack?.Invoke();
        }

        private bool TryGetAnimationEvents(AnimationClip animationClip, out AnimationEvent[] animationEvents)
        {
            if (animationClip == null)
            {
                Debug.LogError("애니메이션 클립이 지정되지 않았습니다.");
                animationEvents = null;
                return false;
            }

            // 애니메이션 클립에 있는 이벤트들 가져오기
            animationEvents = AnimationUtility.GetAnimationEvents(animationClip);
#if UNITY_EDITOR
            // 이벤트 정보 출력
            foreach (AnimationEvent animationEvent in animationEvents)
            {
                Debug.Log($"이벤트 함수 이름: {animationEvent.functionName}, 시간: {animationEvent.time}");
            }
#endif
            if (animationEvents.Length > 0)
                return true;
            return false;
        }

        private void CheckAnimationEventCorrectly(AnimationClip clip, AnimationEvent[] animationEvents)
        {
            bool[] isCheck = new bool[2];
            for (int i = 0; i < animationEvents.Length; ++i)
            {
                if (animationEvents[i].functionName == "AnimationStartHandler")
                    isCheck[0] = true;
                else if (animationEvents[i].functionName == "AnimationEndHandler")
                    isCheck[1] = true;
            }

            if (!isCheck[0])
                AddEvent(clip, 0, "AnimationStartHandler");
            if (!isCheck[1])
                AddEvent(clip, clip.length, "AnimationEndHandler");
        }

        private void AddEvent(AnimationClip clip, float position, string functionName)
        {

            var animationStartEvent = new AnimationEvent
            {
                time = position - 0.1f,
                functionName = functionName,
                stringParameter = clip.name
            };

            clip.AddEvent(animationStartEvent);
        }
    }
}