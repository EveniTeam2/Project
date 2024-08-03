using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Character.States;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs
{
    [CreateAssetMenu(fileName = nameof(CharacterHitStateDTO), menuName = "State/" + nameof(CharacterHitStateDTO))]
    public class CharacterHitStateDTO : CharacterBaseStateDto
    {
        [Header("Hit State Info")]
        [SerializeField] private HitStateInfoDTO hitStateInfoDto;

        public override IState BuildState(StateMachine stateMachine, ICharacterFsmController fsmController, AnimatorSystem animatorSystem,
            Dictionary<AnimationParameterEnums, int> animationParameterHash)
        {
            return new CharacterHitState(characterBaseStateInfoDto.GetInfo(animationParameterHash), hitStateInfoDto.GetInfo(), stateMachine.TryChangeState, animatorSystem, fsmController);
        }
    }

    [Serializable]
    public struct HitStateInfoDTO
    {
        public StateType defaultStateType;
        public HitStateInfo GetInfo()
        {
            return new HitStateInfo(defaultStateType);
        }
    }

    public struct HitStateInfo
    {
        public readonly StateType DefaultStateType;

        public HitStateInfo(StateType defaultStateType)
        {
            DefaultStateType = defaultStateType;
        }
    }
}