using System.Collections;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures
{
    public abstract class Creature : MonoBehaviour, ICreatureServiceProvider, ICreatureStatModifier
    {
        /// <summary>
        ///     애니메이터를 반환합니다.
        /// </summary>
        protected AnimatorEventReceiver _animatorEventReceiver;

        /// <summary>
        ///     전투에 관한 시스템을 반환합니다.
        /// </summary>
        protected BattleSystem _battleSystem;

        /// <summary>
        ///     StateMachine을 반환합니다.
        /// </summary>
        protected StateMachine _fsm;

        /// <summary>
        ///     방어에 관한 시스템을 반환합니다.
        /// </summary>
        protected HealthSystem _healthSystem;

        /// <summary>
        ///     변경된 스탯 리스트입니다.
        /// </summary>
        protected LinkedList<ModifyStatData> _mods;

        /// <summary>
        ///     이동에 관한 시스템을 반환합니다.
        /// </summary>
        protected MovementSystem _movementSystem;

        ICreatureStatModifier ICreatureServiceProvider.StatModifier => this;

        ICreatureBattle ICreatureServiceProvider.BattleSystem => _battleSystem;

        ICreatureHeath ICreatureServiceProvider.HeathSystem => _healthSystem;

        ICreatureMovement ICreatureServiceProvider.MovementSystem => _movementSystem;

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
            var node = _mods.AddLast(data);
            ModifyStat(statType, value);
            while (duration <= 0)
            {
                duration -= Time.deltaTime;
                data.Duration = duration;
                yield return null;
            }

            _mods.Remove(node);
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
        None = 0,
        Health = 1,
        Attack = 2,
        Speed = 3
    }
}