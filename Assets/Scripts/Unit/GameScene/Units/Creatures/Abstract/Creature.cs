using System.Collections;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public abstract class Creature : MonoBehaviour
    {
        /// <summary>
        ///     변경된 스탯 리스트입니다.
        /// </summary>
        protected LinkedList<ModifyStatData> Mods;
        
        /// <summary>
        ///     StateMachine을 반환합니다.
        /// </summary>
        public StateMachine FsmSystem;

        /// <summary>
        ///     애니메이터를 반환합니다.
        /// </summary>
        protected AnimatorSystem AnimatorSystem;

        public abstract HealthSystem BaseHealthSystem { get; }
        public abstract BattleSystem BaseBattleSystem{ get; }
        public abstract MovementSystem BaseMovementSystem { get; }

        /// <summary>
        ///     영구적으로 현재 스탯을 변경합니다.
        /// </summary>
        /// <param name="statType">스탯 종류</param>
        /// <param name="value">변환 값</param>
        public abstract void PermanentModifyStat(EStatType statType, int value);

        /// <summary>
        ///     임시로 현재 스탯을 변경합니다.
        /// </summary>
        /// <param name="statType">스탯 종류</param>
        /// <param name="value">변환 값</param>
        /// <param name="duration">지속 시간</param>
        public abstract void TempModifyStat(EStatType statType, int value, float duration);

        /// <summary>
        ///     변경된 스텟을 전부 지웁니다.
        /// </summary>
        public abstract void ClearModifiedStat();

        protected abstract void ModifyStat(EStatType statType, int value);

        protected IEnumerator TempModifyStatCoroutine(EStatType statType, int value, float duration)
        {
            var data = new TempModifyStatData(statType, value, duration);
            var node = Mods.AddLast(data);
            ModifyStat(statType, value);
            while (duration <= 0)
            {
                duration -= Time.deltaTime;
                data.Duration = duration;
                yield return null;
            }

            Mods.Remove(node);
            ModifyStat(statType, -value);
        }
    }

    /// <summary>
    /// </summary>
    public class ModifyStatData
    {
        /// <summary>
        /// </summary>
        /// <param name="statType"></param>
        /// <param name="value"></param>
        public ModifyStatData(EStatType statType, int value)
        {
            StatType = statType;
            Value = value;
        }

        /// <summary>
        /// </summary>
        public EStatType StatType { get; protected set; }

        /// <summary>
        /// </summary>
        public int Value { get; protected set; }
    }

    /// <summary>
    /// </summary>
    public class TempModifyStatData : ModifyStatData
    {
        /// <summary>
        /// </summary>
        /// <param name="statType"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        public TempModifyStatData(EStatType statType, int value, float duration) : base(statType, value)
        {
            Duration = duration;
        }

        /// <summary>
        /// </summary>
        public float Duration { get; set; }
    }

    /// <summary>
    /// </summary>
    public enum EStatType
    {
        None,
        CurrentHealth,
        MaxHP,
        CurrentExp,
        MaxExp,
        CurrentShield,
        MaxShield,
        Damage,
        Speed,
        CardTrigger,
        CoolTime,
        Health,
        Attack,
    }
}