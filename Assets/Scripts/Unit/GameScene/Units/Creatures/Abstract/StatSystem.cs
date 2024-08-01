using System;
using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public abstract class StatSystem
    {
        protected Action<int, int> OnUpdateHealthBarUI;
        
        private event Action OnHit;
        private event Action OnDeath;

        public int CurrentHp { get; protected set; }
        public int MaxHp { get; protected set; }
        public int CurrentShield { get; protected set; }
        public int MaxShield { get; protected set; }
        public int Damage { get; protected set; }
        public int Speed { get; protected set; }

        public abstract void InitializeStat();
        public abstract void HandleUpdateStat(StatType type, float value);

        protected void UpdateCurrentHealthValue(int value)
        {
            if (value < 0)
            {
                OnHit.Invoke();
            }
            
            var tempHealth = CurrentHp + value;

            if (tempHealth > MaxHp)
            {
                CurrentHp = MaxHp;
            }
            else if (tempHealth < 0)
            {
                CurrentHp = 0;
                OnDeath.Invoke();
            }
            else
            {
                CurrentHp += value;
            }
            
            OnUpdateHealthBarUI.Invoke(CurrentHp, MaxHp);
        }

        protected void UpdateCurrentShieldValue(int value)
        {
            var tempShield = CurrentShield + value;

            if (tempShield > MaxShield)
            {
                CurrentShield = MaxShield;
            }
            else if (tempShield < 0)
            {
                CurrentShield = 0;
            }
            else
            {
                CurrentShield += value;
            }
        }

        protected void UpdateSpeedValue(int value)
        {
            var tempSpeed = Speed + value;

            // TODO : 최소 스피드
            const int minimumSpeed = 1;
            Speed = tempSpeed < 0 ? minimumSpeed : tempSpeed;
        }

        protected void UpdateDamageValue(int value)
        {
            // TODO : 공격력 감소 디버프가 있을 때, 0으로 내려갔다가 복구될 경우 이전 수치보다 높아질 가능성이 있음
            var tempDamage = Damage + value;

            if (tempDamage < 0)
            {
                Damage = 0;
            }
            else
            {
                Damage += value;
            }
        }

        protected void UpdateMaxShieldValue(int value)
        {
            var tempShield = MaxShield + value;

            if (tempShield < CurrentShield)
            {
                CurrentShield = tempShield;
            }
            else if (tempShield <= 0)
            {
                MaxShield = 0;
                CurrentShield = 0;
            }
            else if (value > 0)
            {
                MaxShield += value;
                UpdateCurrentShieldValue(value);
            }
            else
            {
                MaxShield += value;
            }
        }

        public void RegisterHandleOnHit(Action action)
        {
            OnHit += action;
        }

        public void RegisterHandleOnDeath(Action action)
        {
            OnDeath += action;
        }

        public void RegisterHandleOnUpdateHpUI(Action<int, int> action)
        {
            OnUpdateHealthBarUI += action;
        }
    }
}