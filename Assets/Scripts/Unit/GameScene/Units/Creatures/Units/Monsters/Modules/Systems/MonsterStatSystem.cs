using System;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Stats;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Systems
{
    public class MonsterStatSystem : StatSystem
    {
        private MonsterStat _monsterStat;
        public float AttackCoolTime { get; private set; }
        
        public MonsterStatSystem(MonsterStat stat)
        {
            _monsterStat = stat;
            
            InitializeStat();
        }

        public sealed override void InitializeStat()
        {
            CurrentHp = _monsterStat.health;
            MaxHp = _monsterStat.health;
            CurrentShield = 0;
            MaxShield = 0;
            Damage = _monsterStat.damage;
            Speed = _monsterStat.speed;
            AttackCoolTime = _monsterStat.attackCoolTime;
        }

        public override void HandleUpdateStat(StatType type, float value)
        {
            switch (type)
            {
                case StatType.CurrentHp:
                    UpdateCurrentHealthValue((int) value);
                    break;
                case StatType.CurrentShield:
                    UpdateCurrentShieldValue((int) value);
                    break;
                case StatType.MaxShield:
                    UpdateMaxShieldValue((int) value);
                    break;
                case StatType.Damage:
                    UpdateDamageValue((int) value);
                    break;
                case StatType.Speed:
                    UpdateSpeedValue((int) value);
                    break;
                case StatType.AttackCoolTime:
                    UpdateAttackCoolTimeValue(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            Debug.Log($"Monster Stat {type.ToString()} {value}로 변동");
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
    }
}