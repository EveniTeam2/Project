using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Stages.Creautres.Module
{
    public class AnimationController : MonoBehaviour, IAnimationController
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        ///     달리는 애니메이션을 설정합니다.
        /// </summary>
        /// <param name="isRunning">달리는 상태인지 여부</param>
        public void SetRunning(bool isRunning)
        {
            _animator.SetBool("isRunning", isRunning);
        }

        /// <summary>
        ///     점프 애니메이션을 트리거합니다.
        /// </summary>
        public void TriggerJump()
        {
            _animator.SetTrigger("Jump");
        }

        /// <summary>
        ///     공격 애니메이션을 트리거합니다.
        /// </summary>
        public void TriggerAttack()
        {
            _animator.SetTrigger("Attack");
        }
    }
}