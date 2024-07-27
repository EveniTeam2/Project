using System;
using System.Collections.Generic;
using TMPro;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public abstract class BaseStateDTO : ScriptableObject
    {
        [Header("Default State Info")]
        [SerializeField] protected BaseStateInfoDTO baseStateInfoDTO;
        public abstract IState Build(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, AnimatorEventReceiver animatorEventReceiver, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa);
    }
    public interface IStateInfoDTO
    {
        public BaseStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums);
    }

    [Serializable]
    public struct BaseStateInfoDTO : IStateInfoDTO
    {
        public StateType stateType;
        public AnimationParameterEnums stateParameter;

        public BaseStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new BaseStateInfo(stateType, animationParameterEnums[stateParameter]);
        }
    }

    public struct BaseStateInfo
    {
        public StateType stateType;
        public int stateParameter;

        public BaseStateInfo(StateType stateType, int stateParameter)
        {
            this.stateType = stateType;
            this.stateParameter = stateParameter;
        }
    }
}