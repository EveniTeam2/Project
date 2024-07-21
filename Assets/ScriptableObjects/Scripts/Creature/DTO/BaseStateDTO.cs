using System;
using System.Collections.Generic;
using TMPro;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Units.Creatures.Module;
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
    public class BaseStateInfoDTO : IStateInfoDTO
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