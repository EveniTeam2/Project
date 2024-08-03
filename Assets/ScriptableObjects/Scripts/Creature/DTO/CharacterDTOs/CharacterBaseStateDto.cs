using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Character.DTO.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs
{
    public abstract class CharacterBaseStateDto : ScriptableObject
    {
        [Header("Default State Info")]
        [SerializeField] protected CharacterBaseStateInfoDto characterBaseStateInfoDto;
        
        public abstract IState BuildState(StateMachine stateMachine, ICharacterFsmController fsmController, AnimatorSystem animatorSystem, Dictionary<AnimationParameterEnums, int> animationParameterHash);
    }

    [Serializable]
    public struct CharacterBaseStateInfoDto : IStateInfoDTO
    {
        public StateType stateType;
        public AnimationParameterEnums stateParameter;

        public CharacterBaseStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new CharacterBaseStateInfo(stateType, animationParameterEnums[stateParameter]);
        }
    }

    public struct CharacterBaseStateInfo
    {
        public readonly StateType StateType;
        public readonly int StateParameter;

        public CharacterBaseStateInfo(StateType stateType, int stateParameter)
        {
            StateType = stateType;
            StateParameter = stateParameter;
        }
    }
}