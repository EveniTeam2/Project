using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public abstract class MonsterBaseStateDTO : ScriptableObject
    {
        [Header("Default State Info")]
        [SerializeField] protected MonsterBaseStateinfoDTO monsterBaseStateInfoDTO;
        public abstract IState BuildMonster(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, StateMachine st, AnimatorEventReceiver animatorEventReceiver, Dictionary<AnimationParameterEnums, int> anPa);
    }

    [Serializable]
    public struct MonsterBaseStateinfoDTO
    {
        public StateType stateType;
        public AnimationParameterEnums stateParameter;
        public StateType _defaultExitState;

        public MonsterBaseStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new MonsterBaseStateInfo(stateType, animationParameterEnums[stateParameter], _defaultExitState);
        }
    }

    public struct MonsterBaseStateInfo
    {
        public StateType stateType;
        public int stateParameter;
        public StateType _defaultExitState;

        public MonsterBaseStateInfo(StateType stateType, int stateParameter, StateType defaultExitState)
        {
            _defaultExitState = defaultExitState;
            this.stateType = stateType;
            this.stateParameter = stateParameter;
        }
    }
}