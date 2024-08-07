using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Cards.Interfaces;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public abstract class Creature : MonoBehaviour
    {
        public StateMachine FsmSystem { get; protected set; }
        protected Dictionary<AnimationParameterEnums, int> AnimationParameters { get; set; }

        protected abstract AnimatorSystem AnimatorSystem { get; set; }
        protected abstract Collider2D CreatureCollider { get; set; }
        protected abstract RectTransform CreatureHpHandler { get; set; }
        protected abstract RectMask2D CreatureHpHandlerMask { get; set; }

        protected abstract void RegisterEventHandler();
        protected abstract void HandleOnHit();
        protected abstract void HandleOnDeath();

        protected virtual void UpdateHpBar(int currentHp, int maxHp)
        {
            float healthRatio = (float)currentHp / maxHp;
            float rightPadding = CreatureHpHandler.rect.width * (1 - healthRatio);
            CreatureHpHandlerMask.padding = new Vector4(0, 0, rightPadding, 0);
        }

        protected void SetActiveCollider(bool active)
        {
            if (CreatureCollider != null)
            {
                CreatureCollider.enabled = active;
            }
        }

        public void SetBool(int parameter, bool value, Action action)
        {
            AnimatorSystem.SetBool(parameter, value, action);
        }

        public void SetTrigger(int parameter, Action action)
        {
            AnimatorSystem.SetTrigger(parameter, action);
        }

        public void SetFloat(int parameter, int value, Action action)
        {
            AnimatorSystem.SetFloat(parameter, value, action);
        }

        public void SetBoolOnAnimator(AnimationParameterEnums targetParameter, bool value, Action action)
        {
            AnimatorSystem.SetBool(targetParameter, value, action);
        }

        public void SetFloatOnAnimator(AnimationParameterEnums targetParameter, int value, Action action)
        {
            AnimatorSystem.SetFloat(targetParameter, value, action);
        }
    }
}