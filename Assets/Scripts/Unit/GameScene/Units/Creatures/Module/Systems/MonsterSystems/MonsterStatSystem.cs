using System;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems
{
    public class MonsterStatSystem : StatSystem
    {
        protected override MonoBehaviour MonoBehaviour { get; set; }
        public override int CurrentHp { get; protected set; }
        public override int MaxHp { get; protected set; }
        public override int CurrentShield { get; protected set; }
        public override int MaxShield { get; protected set; }
        public override int Damage { get; protected set; }
        public override int Speed { get; protected set; }
        public float AttackCoolTime { get; private set; }
        
        private MonsterStat _monsterStat;

        public MonsterStatSystem(MonsterStat stat)
        {
            _monsterStat = stat;
        }

        public override void InitializeStat(MonoBehaviour monoBehaviour)
        {
            MonoBehaviour = monoBehaviour;
            CurrentHp = _monsterStat.health;
            MaxHp = _monsterStat.health;
            CurrentShield = 0;
            MaxShield = 0;
            Damage = _monsterStat.damage;
            Speed = _monsterStat.speed;
            AttackCoolTime = _monsterStat.attackCoolTime;

            OnUpdateHpPanelUI?.Invoke(CurrentHp, MaxHp);
        }
        protected override void HandleOnUpdateStat(StatType type, float value)
        {
            switch (type)
            {
                case StatType.CurrentHp:
                    UpdateCurrentHealthValue((int)value);
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
                case StatType.AttackCoolTime:
                    UpdateAttackCoolTimeValue(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void UpdateAttackCoolTimeValue(float value)
        {
            AttackCoolTime = Mathf.Max(AttackCoolTime + value, 0);
        }

        public int ReturnExp()
        {
            return CurrentHp <= 0 ? _monsterStat.returnExp : 0;
        }
    }
}