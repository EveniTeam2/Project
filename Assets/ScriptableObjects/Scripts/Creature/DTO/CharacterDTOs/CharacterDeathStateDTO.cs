using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Character.States;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs
{
    [CreateAssetMenu(fileName = nameof(CharacterDeathStateDTO), menuName = "State/" + nameof(CharacterDeathStateDTO))]
    public class CharacterDeathStateDTO : CharacterBaseStateDto
    {
        [Header("Death State Info")]
        [SerializeField] DeathStateInfoDTO deathStateInfoDto;
        
        public override IState BuildState(StateMachine stateMachine, ICharacterFsmController fsmController, AnimatorSystem animatorSystem, Dictionary<AnimationParameterEnums, int> animationParameterHash)
        {
            return new CharacterDeathState(characterBaseStateInfoDto.GetInfo(animationParameterHash), deathStateInfoDto.GetInfo(), stateMachine.TryChangeState, animatorSystem, fsmController);
        }
    }
}