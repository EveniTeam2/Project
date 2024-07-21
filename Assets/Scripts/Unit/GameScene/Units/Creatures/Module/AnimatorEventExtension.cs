using System;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module
{
   public static class AnimatorEventExtension
    {
        private static AnimatorEventReceiver AttachReceiver(ref Animator animator)
        {
            var receiver = animator.gameObject.GetComponent<AnimatorEventReceiver>();
            
            if (receiver == null) receiver = animator.gameObject.AddComponent<AnimatorEventReceiver>();
            
            return receiver;
        }

        public static void SetInteger(this Animator animator, string name, int value, Action onFinished)
        {
            animator.SetInteger(name, value);
            AttachReceiver(ref animator).OnStateEnd(onFinished);
        }

        public static void SetInteger(this Animator animator, int id, int value, Action onFinished)
        {
            animator.SetInteger(id, value);
            AttachReceiver(ref animator).OnStateEnd(onFinished);
        }

        public static void SetFloat(this Animator animator, string name, float value, Action onFinished)
        {
            animator.SetFloat(name, value);
            AttachReceiver(ref animator).OnStateEnd(onFinished);
        }

        public static void SetFloat(this Animator animator, int id, float value, Action onFinished)
        {
            animator.SetFloat(id, value);
            AttachReceiver(ref animator).OnStateEnd(onFinished);
        }

        public static void SetBool(this Animator animator, string name, bool value, Action onFinished)
        {
            animator.SetBool(name, value);
            AttachReceiver(ref animator).OnStateEnd(onFinished);
        }

        public static void SetBool(this Animator animator, int id, bool value, Action onFinished)
        {
            animator.SetBool(id, value);
            AttachReceiver(ref animator).OnStateEnd(onFinished);
        }

        public static void SetTrigger(this Animator animator, string name, Action onFinished)
        {
            animator.SetTrigger(name);
            AttachReceiver(ref animator).OnStateEnd(onFinished);
        }

        public static void SetTrigger(this Animator animator, int id, Action onFinished)
        {
            animator.SetTrigger(id);
            AttachReceiver(ref animator).OnStateEnd(onFinished);
        }
    }
}