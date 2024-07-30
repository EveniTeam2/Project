using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Character.States;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs
{
    [CreateAssetMenu(fileName = nameof(CharacterIdleStateDTO), menuName = "State/" + nameof(CharacterIdleStateDTO))]
    public class CharacterIdleStateDTO : CharacterBaseStateDto
    {
        [Header("Idle State Info")]
        [SerializeField] private IdleStateInfoDTO idleInfoDto;
        
        public override IState BuildState(StateMachine stateMachine, ICharacterFsmController fsmController, AnimatorSystem animatorSystem, Dictionary<AnimationParameterEnums, int> animationParameterHash)
        {
            return new CharacterIdleState(characterBaseStateInfoDto.GetInfo(animationParameterHash), idleInfoDto.GetInfo(animationParameterHash), stateMachine.TryChangeState, animatorSystem, fsmController);
        }
    }

    [Serializable]
    public struct IdleStateInfoDTO
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public IdleStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new IdleStateInfo(targetLayer, direction, distance);
        }
    }

    public struct IdleStateInfo
    {
        public readonly float Distance;
        
        public LayerMask TargetLayer;
        public Vector2 Direction;

        public IdleStateInfo(LayerMask targetLayer, Vector2 direction, float distance) : this()
        {
            TargetLayer = targetLayer;
            Direction = direction;
            Distance = distance;
        }
    }
}