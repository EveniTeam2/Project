using System;
using System.Collections.Generic;
using Unit.Stages.Creatures.FSM;
using Unit.Stages.Creatures.Interfaces;
using UnityEngine;

namespace Unit.Stages.Creatures {
    public abstract class BaseCreature : MonoBehaviour {
        /// <summary>
        /// StateMachine을 반환합니다.
        /// </summary>
        public StateMachine HFSM { get; protected set; }
        /// <summary>
        /// 전투에 관한 시스템을 반환합니다.
        /// </summary>
        public abstract BattleSystem Battle { get; }
        /// <summary>
        /// 이동에 관한 시스템을 반환합니다.
        /// </summary>
        public abstract IRunnable Movement { get; }
        /// <summary>
        /// 방어에 관한 시스템을 반환합니다.
        /// </summary>
        public abstract IDamageable Health { get; }
        /// <summary>
        /// 애니메이터를 반환합니다.
        /// </summary>
        public abstract Animator Animator { get; }
        /// <summary>
        /// 영구적으로 현재 스탯을 변경합니다.
        /// </summary>
        /// <param name="statType">스탯 종류</param>
        /// <param name="value">변환 값</param>
        public abstract void PermanentModifyStat(EStatType statType, int value);
        /// <summary>
        /// 임시로 현재 스탯을 변경합니다.
        /// </summary>
        /// <param name="statType">스탯 종류</param>
        /// <param name="value">변환 값</param>
        /// <param name="duration">지속 시간</param>
        public abstract void TempModifyStat(EStatType statType, int value, float duration);
        /// <summary>
        /// 변경된 스탯 리스트입니다.
        /// </summary>
        public abstract LinkedList<ModifyStatData> ModifiedStatData { get; }
    }

    public class ModifyStatData {
        public EStatType StatType { get; protected set; }
        public int Value { get; protected set; }
        public ModifyStatData(EStatType statType, int value) {
            StatType = statType;
            Value = value;
        }
    }

    public class TempModifyStatData : ModifyStatData {
        public float Duration { get; set; }
        public TempModifyStatData(EStatType statType, int value, float duration) : base(statType, value) {
            Duration = duration;
        }
    }

    public enum EStatType {
        None = 0,
        Health = 1,
        Attack = 2,
        Speed = 3,
    }
}