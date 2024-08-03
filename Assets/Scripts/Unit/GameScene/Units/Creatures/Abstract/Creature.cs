using System;
using System.Collections;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public interface IUpdateStat
    {
        public Action<StatType, float> OnUpdateStat { get; set; }
    }
    
    public abstract class Creature : MonoBehaviour, IUpdateStat
    {
        public Action<StatType, float> OnUpdateStat { get; set; }
        
        public StateMachine FsmSystem;
        
        protected abstract AnimatorSystem AnimatorSystem { get; set; }
        protected abstract Collider2D CreatureCollider { get; set; }
        protected abstract RectTransform CreatureHpPanelUI { get; set; }
        
        protected abstract void RegisterEventHandler();
        protected abstract void HandleOnHit();
        protected abstract void HandleOnDeath();

        protected Dictionary<AnimationParameterEnums, int> AnimationParameters;
        
        protected void HandleOnUpdateHpPanel(int currentHp, int maxHp)
        {
            Debug.Log($"currentHp {currentHp} / maxHp {maxHp}");
            // 계산된 체력 비율
            float healthRatio = (float)currentHp / maxHp;
    
            // 새로운 localScale 값 계산
            var newScale = new Vector3(healthRatio, CreatureHpPanelUI.localScale.y, CreatureHpPanelUI.localScale.z);
    
            // 체력 바의 스케일을 업데이트
            CreatureHpPanelUI.localScale = newScale;
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