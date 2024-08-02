using System;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Systems
{
    public class MonsterStatSystem : StatSystem
    {
        private event Action<int> OnIncreasePlayerExp;
        
        private readonly MonsterStat _monsterStat;
        public float AttackCoolTime { get; private set; }
        
        public MonsterStatSystem(MonsterStat stat)
        {
            _monsterStat = stat;
        }

        public override void InitializeStat()
        {
            CurrentHp = _monsterStat.health;
            MaxHp = _monsterStat.health;
            CurrentShield = 0;
            MaxShield = 0;
            Damage = _monsterStat.damage;
            Speed = _monsterStat.speed;
            AttackCoolTime = _monsterStat.attackCoolTime;

            OnUpdateHpPanelUI.Invoke(CurrentHp, MaxHp);
        }

        public override void HandleOnUpdateStat(StatType type, float value)
        {
            switch (type)
            {
                case StatType.CurrentHp:
                    Debug.Log($"몬스터 Stat {type.ToString()} 현재 {CurrentHp}");
                    UpdateCurrentHealthValue((int) value);
                    InvokeOnIncreasePlayerExp();
                    Debug.Log($"몬스터 Stat {type.ToString()} {value} => {CurrentHp}로 변동");
                    break;
                case StatType.CurrentShield:
                    Debug.Log($"몬스터 Stat {type.ToString()} 현재 {CurrentShield}");
                    UpdateCurrentShieldValue((int) value);
                    Debug.Log($"몬스터 Stat {type.ToString()} {value} => {CurrentShield}로 변동");
                    break;
                case StatType.MaxShield:
                    Debug.Log($"몬스터 Stat {type.ToString()} 현재 {MaxShield}");
                    UpdateMaxShieldValue((int) value);
                    Debug.Log($"몬스터 Stat {type.ToString()} {value} => {MaxShield}로 변동");
                    break;
                case StatType.Damage:
                    Debug.Log($"몬스터 Stat {type.ToString()} 현재 {Damage}");
                    UpdateDamageValue((int) value);
                    Debug.Log($"몬스터 Stat {type.ToString()} {value} => {Damage}로 변동");
                    break;
                case StatType.Speed:
                    Debug.Log($"몬스터 Stat {type.ToString()} 현재 {Speed}");
                    UpdateSpeedValue((int) value);
                    Debug.Log($"몬스터 Stat {type.ToString()} {value} => {Speed}로 변동");
                    break;
                case StatType.AttackCoolTime:
                    Debug.Log($"몬스터 Stat {type.ToString()} 현재 {AttackCoolTime}");
                    UpdateAttackCoolTimeValue(value);
                    Debug.Log($"몬스터 Stat {type.ToString()} {value} => {AttackCoolTime}로 변동");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            Debug.Log($"Monster Stat {type.ToString()} {value}로 변동");
        }

        private void InvokeOnIncreasePlayerExp()
        {
            if (CurrentHp <= 0 ) OnIncreasePlayerExp.Invoke(_monsterStat.returnExp);
            OnIncreasePlayerExp = null;
        }

        private void UpdateAttackCoolTimeValue(float value)
        {
            var tempAttackCoolTime = AttackCoolTime + value;

            if (tempAttackCoolTime < 0)
            {
                AttackCoolTime = 0;
            }
            else
            {
                AttackCoolTime += value;
            }
        }

        public void RegisterHandleOnIncreasePlayerExp(Action<int> action)
        {
            OnIncreasePlayerExp = action;
        }
    }
}