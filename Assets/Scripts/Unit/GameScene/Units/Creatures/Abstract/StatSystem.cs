using System;
using System.Collections;
using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public abstract class StatSystem : MonoBehaviour
    {
        protected Action<int, int> OnUpdateHpPanelUI;
        
        private event Action OnHit;
        private event Action OnDeath;

        protected abstract MonoBehaviour MonoBehaviour { get; set; }
        public abstract int CurrentHp { get; protected set; }
        public abstract int MaxHp { get; protected set; }
        public abstract int CurrentShield { get; protected set; }
        public abstract int MaxShield { get; protected set; }
        public abstract int Damage { get; protected set; }
        public abstract int Speed { get; protected set; }
        
        public abstract void InitializeStat(MonoBehaviour monoBehaviour);
        protected abstract void HandleOnUpdateStat(StatType type, float value);

        protected void UpdateCurrentHealthValue(int value)
        {
            if (value < 0)
            {
                OnHit?.Invoke();
            }
            
            CurrentHp = Mathf.Clamp(CurrentHp + value, 0, MaxHp);
            
            if (CurrentHp <= 0)
            {
                OnDeath?.Invoke();
            }
            
            OnUpdateHpPanelUI?.Invoke(CurrentHp, MaxHp);
        }

        protected void UpdateCurrentShieldValue(int value)
        {
            CurrentShield = Mathf.Clamp(CurrentShield + value, 0, MaxShield);
        }

        protected void UpdateSpeedValue(int value)
        {
            const int minimumSpeed = 1;
            Speed = Mathf.Max(Speed + value, minimumSpeed);
        }

        protected void UpdateDamageValue(int value)
        {
            Damage = Mathf.Max(Damage + value, 0);
        }

        protected void UpdateMaxShieldValue(int value)
        {
            MaxShield = Mathf.Max(MaxShield + value, 0);
            CurrentShield = Mathf.Min(CurrentShield, MaxShield);
        }

        public void RegisterHandleOnHit(Action action)
        {
            OnHit += action;
        }

        public void RegisterHandleOnDeath(Action action)
        {
            OnDeath += action;
        }

        public void RegisterHandleOnUpdateHpPanelUI(Action<int, int> action)
        {
            OnUpdateHpPanelUI += action;
        }
        
        public void RegisterHandleOnUpdatePermanentStat(StatType type, float value)
        {
            HandleOnUpdateStat(type, value);
        }
        
        public void RegisterHandleOnUpdateTemporaryStat(StatType type, float value, float duration)
        {
            StartCoroutine(BuffTimer(type, value, duration));
        }

        private IEnumerator BuffTimer(StatType type, float value, float duration)
        {
            HandleOnUpdateStat(type, value);
            
            yield return new WaitForSeconds(duration);
            
            HandleOnUpdateStat(type, -value);
        }
    }
}