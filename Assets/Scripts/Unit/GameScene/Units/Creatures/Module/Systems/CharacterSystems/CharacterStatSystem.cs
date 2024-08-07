using System;
using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Data.CharacterDatas;
using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems
{
    public class CharacterStatSystem : StatSystem
    {
        public Action<int> OnIncreasePlayerExp;
        
        public event Action<int, int> OnUpdateExpPanelUI;
        public event Action<int> OnUpdateLevelPanelUI;
        
        public event Action OnTriggerCard;

        private readonly Dictionary<int, CharacterStatData> _entireStatInfo;

        protected override MonoBehaviour MonoBehaviour { get; set; }
        public override int CurrentHp { get; protected set; }
        public override int MaxHp { get; protected set; }
        public override int CurrentShield { get; protected set; }
        public override int MaxShield { get; protected set; }
        public override int Damage { get; protected set; }
        public override int Speed { get; protected set; }
        public int CurrentLevel { get; private set; }
        public int MaxLevel { get; private set; }
        public int CurrentExp { get; private set; }
        public int MaxExp { get; private set; }
        public int CardTrigger { get; private set; }

        public CharacterStatSystem(CharacterClassType characterClassType, List<CharacterStatData> statDataList)
        {
            _entireStatInfo = statDataList
                .Where(statData => characterClassType == statData.CharacterType)
                .ToDictionary(statData => statData.CharacterLevel);
        }

        public override void InitializeStat(MonoBehaviour monoBehaviour)
        {
            var initialStat = _entireStatInfo[0];

            MonoBehaviour = monoBehaviour;
            CurrentLevel = initialStat.CharacterLevel;
            MaxLevel = _entireStatInfo.Count;
            CurrentExp = 0;
            MaxExp = initialStat.CharacterMaxExp;
            CurrentHp = initialStat.CharacterMaxHp;
            MaxHp = initialStat.CharacterMaxHp;
            CurrentShield = initialStat.CharacterMaxShield;
            MaxShield = initialStat.CharacterMaxShield;
            Damage = initialStat.CharacterDamage;
            Speed = initialStat.CharacterSpeed;
            CardTrigger = initialStat.CardTrigger;

            OnUpdateHpPanelUI?.Invoke(CurrentHp, MaxHp);
            OnUpdateExpPanelUI?.Invoke(CurrentExp, MaxExp);
            OnUpdateLevelPanelUI?.Invoke(CurrentLevel);
            
            OnIncreasePlayerExp += HandleOnIncreaseExp;
        }

        protected override void HandleOnUpdateStat(StatType type, float value)
        {
            switch (type)
            {
                case StatType.CurrentExp:
                    UpdateCurrentExpValue((int)value);
                    break;
                case StatType.CurrentHp:
                    UpdateCurrentHealthValue((int)value);
                    break;
                case StatType.MaxHp:
                    UpdateMaxHealthValue((int)value);
                    break;
                case StatType.CurrentShield:
                    UpdateCurrentShieldValue((int)value);
                    break;
                case StatType.MaxShield:
                    UpdateMaxShieldValue((int)value);
                    break;
                case StatType.Damage:
                    UpdateDamageValue((int)value);
                    break;
                case StatType.Speed:
                    UpdateSpeedValue((int)value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void UpdateMaxHealthValue(int value)
        {
            MaxHp = Mathf.Max(MaxHp + value, 1);
            CurrentHp = Mathf.Min(CurrentHp, MaxHp);
        }

        private void UpdateCurrentExpValue(int value)
        {
            if (CurrentLevel == MaxLevel && CurrentExp == MaxExp) return;

            CurrentExp += value;

            if (CurrentExp >= MaxExp)
            {
                CurrentExp -= MaxExp;
                CurrentLevel = Mathf.Min(CurrentLevel + 1, MaxLevel);
                UpdateEntireStat(CurrentLevel);
                OnUpdateLevelPanelUI?.Invoke(CurrentLevel);
            }

            OnUpdateExpPanelUI?.Invoke(CurrentExp, MaxExp);
        }

        private void UpdateEntireStat(int level)
        {
            if (!_entireStatInfo.ContainsKey(level)) return;

            var nextStat = _entireStatInfo[level];
            var previousStat = _entireStatInfo[level - 1];

            MaxExp += nextStat.CharacterMaxExp - previousStat.CharacterMaxExp;
            MaxHp += nextStat.CharacterMaxHp - previousStat.CharacterMaxHp;
            MaxShield += nextStat.CharacterMaxShield - previousStat.CharacterMaxShield;
            Damage += nextStat.CharacterDamage - previousStat.CharacterDamage;
            Speed += nextStat.CharacterSpeed - previousStat.CharacterSpeed;
            CardTrigger = nextStat.CardTrigger;

            if (CardTrigger == 1)
            {
                OnTriggerCard?.Invoke();
            }
        }

        private void HandleOnIncreaseExp(int value)
        {
            HandleOnUpdateStat(StatType.CurrentExp, value);
        }

        public void RegisterHandleOnUpdateExpPanelUI(Action<int, int> action)
        {
            OnUpdateExpPanelUI = action;
        }

        public void RegisterHandleOnUpdateLevelPanelUI(Action<int> action)
        {
            OnUpdateLevelPanelUI = action;
        }

        public void RegisterHandleOnTriggerCard(Action action)
        {
            OnTriggerCard += action;
        }
    }
}