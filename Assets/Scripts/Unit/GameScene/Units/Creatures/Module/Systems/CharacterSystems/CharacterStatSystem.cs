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
        private event Action<int, int> OnUpdateExpPanelUI;
        private event Action<int> OnIncreasePlayerLevel;
        public event Action OnTriggerCard;
        
        private readonly Dictionary<int, CharacterStatData> _entireStatInfo;

        public int CurrentLevel { get; private set; }
        public int MaxLevel { get; private set; }
        public int CurrentExp { get; private set; }
        public int MaxExp { get; private set; }
        public int CardTrigger { get; private set; }

        public CharacterStatSystem(CharacterClassType characterClassType, List<CharacterStatData> statDataList)
        {
            _entireStatInfo = new Dictionary<int, CharacterStatData>();
            
            foreach (var statData in statDataList.Where(statData => characterClassType == statData.CharacterType))
            {
                _entireStatInfo.Add(statData.CharacterLevel, statData);
            }
        }

        public override void InitializeStat()
        {
            CurrentLevel = _entireStatInfo[0].CharacterLevel;
            MaxLevel = _entireStatInfo.Count;
            CurrentExp = 0;
            MaxExp = _entireStatInfo[0].CharacterMaxExp;
            
            CurrentHp = _entireStatInfo[0].CharacterMaxHp;
            MaxHp = _entireStatInfo[0].CharacterMaxHp;
            CurrentShield = _entireStatInfo[0].CharacterMaxShield;
            MaxShield = _entireStatInfo[0].CharacterMaxShield;
            Damage = _entireStatInfo[0].CharacterDamage;
            Speed = _entireStatInfo[0].CharacterSpeed;
            CardTrigger = _entireStatInfo[0].CardTrigger;
            
            OnUpdateHp.Invoke(CurrentHp, MaxHp);
            OnUpdateExpPanelUI.Invoke(CurrentExp, MaxExp);
            OnIncreasePlayerLevel.Invoke(CurrentLevel);
            
            OnIncreasePlayerExp += HandleOnIncreaseExp;
        }

        public override void HandleOnUpdateStat(StatType type, float value)
        {
            switch (type)
            {
                case StatType.CurrentExp:
                    Debug.Log($"캐릭터 Stat {type.ToString()} 현재 {CurrentExp}");
                    UpdateCurrentExpValue((int) value);
                    Debug.Log($"캐릭터 Stat {type.ToString()} {value} => {CurrentExp}로 변동");
                    break;
                case StatType.CurrentHp:
                    Debug.Log($"Character Stat {type.ToString()} 현재 {CurrentHp}");
                    UpdateCurrentHealthValue((int) value);
                    Debug.Log($"Character Stat {type.ToString()} {value} 수치 수정 => {CurrentHp} 변동");
                    break;
                case StatType.MaxHp:
                    Debug.Log($"Character Stat {type.ToString()} 현재 {MaxHp}");
                    UpdateMaxHealthValue((int) value);
                    Debug.Log($"Character Stat {type.ToString()} {value} 수치 수정 => {MaxHp} 변동");
                    break;
                case StatType.CurrentShield:
                    Debug.Log($"Character Stat {type.ToString()} 현재 {CurrentShield}");
                    UpdateCurrentShieldValue((int) value);
                    Debug.Log($"Character Stat {type.ToString()} {value} => {CurrentShield}로 변동");
                    break;
                case StatType.MaxShield:
                    Debug.Log($"Character Stat {type.ToString()} 현재 {MaxShield}");
                    UpdateMaxShieldValue((int) value);
                    Debug.Log($"Character Stat {type.ToString()} {value} => {MaxShield}로 변동");
                    break;
                case StatType.Damage:
                    Debug.Log($"Character Stat {type.ToString()} 현재 {Damage}");
                    UpdateDamageValue((int) value);
                    Debug.Log($"Character Stat {type.ToString()} {value} => {Damage}로 변동");
                    break;
                case StatType.Speed:
                    Debug.Log($"Character Stat {type.ToString()} 현재 {Speed}");
                    UpdateSpeedValue((int) value);
                    Debug.Log($"Character Stat {type.ToString()} {value} =>  {Speed}로 변동");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void UpdateMaxHealthValue(int value)
        {
            if (value < 0)
            {
                if (MaxHp - value <= 1)
                {
                    MaxHp = 1;
                    CurrentHp = 1;
                }
                else
                {
                    MaxHp -= value;

                    if (CurrentHp > MaxHp)
                    {
                        UpdateCurrentHealthValue(CurrentHp - MaxHp);
                    }
                }
            }
            else
            {
                MaxHp += value;
                UpdateCurrentHealthValue(value);
            }
        }

        private void UpdateCurrentExpValue(int value)
        {
            if (CurrentLevel == MaxExp && CurrentExp == MaxExp) return;
            
            CurrentExp += value;

            if (CurrentExp >= MaxExp)
            {
                CurrentExp -= MaxExp;
                
                if (CurrentLevel + 1 <= MaxLevel)
                {
                    CurrentLevel++;
                }
                else
                {
                    CurrentLevel = MaxLevel;
                    CurrentExp = MaxExp;
                }
                
                UpdateEntireStat(CurrentLevel);
                OnIncreasePlayerLevel.Invoke(CurrentLevel);
            }
            
            OnUpdateExpPanelUI.Invoke(CurrentExp, MaxExp);
        }

        private void UpdateEntireStat(int level)
        {
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
                OnTriggerCard.Invoke();
            }
        }
        
        private void HandleOnIncreaseExp(int value)
        {
            HandleOnUpdateStat(StatType.CurrentExp, value);
        }

        public void RegisterHandleOnUpdateExpPanelUI(Action<int, int> action)
        {
            OnUpdateExpPanelUI += action;
        }

        public void RegisterHandleOnUpdateLevelPanelUI(Action<int> action)
        {
            OnIncreasePlayerLevel += action;
        }

        public void RegisterHandleOnTriggerCard(Action action)
        {
            OnTriggerCard += action;
        }
    }
}