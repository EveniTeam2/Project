using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Character.States;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs
{
    [CreateAssetMenu(fileName = nameof(CharacterRunStateDTO), menuName = "State/" + nameof(CharacterRunStateDTO))]
    public class CharacterRunStateDTO : CharacterBaseStateDto
    {
        [Header("Default State Info")] [SerializeField]
        private RunStateInfoDTO runStateInfoDTO;

        public override IState BuildState(StateMachine stateMachine, ICharacterFsmController fsmController,
            AnimatorSystem animatorSystem,
            Dictionary<AnimationParameterEnums, int> animationParameterHash)
        {
            return new CharacterRunState(characterBaseStateInfoDto.GetInfo(animationParameterHash),
                runStateInfoDTO.GetInfo(), stateMachine.TryChangeState, animatorSystem, fsmController);
        }
    }

    [Serializable]
    public struct RunStateInfoDTO
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public RunStateInfo GetInfo()
        {
            return new RunStateInfo(distance, direction, targetLayer);
        }
    }

    public struct RunStateInfo
    {
        public readonly float Distance;
        
        public LayerMask TargetLayer;
        public Vector2 Direction;

        public RunStateInfo(float distance, Vector2 direction, LayerMask targetLayer) : this()
        {
            Distance = distance;
            Direction = direction;
            TargetLayer = targetLayer;
        }
    }
}