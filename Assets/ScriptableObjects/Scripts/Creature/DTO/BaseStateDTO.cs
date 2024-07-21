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


    [Serializable]
    public struct BaseStateInfoDTO
    {
        public StateType stateType;
        public AnimationParameterEnums stateParameter;
        public StateType defaultExitState;
        //public float animationTime;
        //public float canTransitTime;
        public BaseStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            //return new BaseStateInfo(stateType, animationParameterEnums[stateParameter], defaultExitState, animationTime, canTransitTime);
            return new BaseStateInfo(stateType, animationParameterEnums[stateParameter], defaultExitState);
        }
    }

    public struct BaseStateInfo
    {
        public StateType stateType;
        public int stateParameter;
        public StateType defaultExitState;
        //public float animationTime;
        //public float canTransitTime;
        //public BaseStateInfo(StateType stateType, int stateParameter, StateType defaultExitState, float animationTime, float canTransitTime)
        public BaseStateInfo(StateType stateType, int stateParameter, StateType defaultExitState)
        {
            this.stateType = stateType;
            this.stateParameter = stateParameter;
            this.defaultExitState = defaultExitState;
            //this.animationTime = animationTime;
            //this.canTransitTime = canTransitTime;
        }
    }
}