using System;
using System.Collections;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public abstract class Creature : MonoBehaviour
    {
        public Action<StatType, float> OnUpdateStat;
        
        public StateMachine FsmSystem;
        protected AnimatorSystem AnimatorSystem;
        
        protected abstract Collider2D CreatureCollider { get; set; }
        protected abstract RectTransform CreatureHpUI { get; set; }
        
        protected abstract void RegisterEventHandler();
        protected abstract void HandleOnHit();
        protected abstract void HandleOnDeath();
        
        protected void HandleOnUpdateHealthBarUI(int currentHp, int maxHp)
        {
            Debug.Log($"currentHp {currentHp} / maxHp {maxHp}");
            // 계산된 체력 비율
            float healthRatio = (float)currentHp / maxHp;
    
            // 새로운 localScale 값 계산
            var newScale = new Vector3(healthRatio, CreatureHpUI.localScale.y, CreatureHpUI.localScale.z);
    
            // 체력 바의 스케일을 업데이트
            CreatureHpUI.localScale = newScale;
        }
        
        protected void SetActiveCollider(bool active)
        {
            if (CreatureCollider != null)
            {
                CreatureCollider.enabled = active;
            }
        }
    }
}