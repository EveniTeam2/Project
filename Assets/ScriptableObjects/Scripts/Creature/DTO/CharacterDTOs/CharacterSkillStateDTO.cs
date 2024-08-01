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
    [CreateAssetMenu(fileName = nameof(CharacterSkillStateDTO), menuName = "State/" + nameof(CharacterSkillStateDTO))]
    public class CharacterSkillStateDTO : CharacterBaseStateDto
    {
        public override IState BuildState(
            StateMachine stateMachine, 
            ICharacterFsmController fsmController, 
            AnimatorSystem animatorSystem,
            Dictionary<AnimationParameterEnums, int> animationParameterHash) 
        {
            return new CharacterSkillState(
                characterBaseStateInfoDto.GetInfo(animationParameterHash), 
                stateMachine.TryChangeState, 
                animatorSystem, 
                fsmController);
        }
    }
}