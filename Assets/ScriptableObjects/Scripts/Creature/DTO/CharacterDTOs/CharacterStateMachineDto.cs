using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs
{
    [CreateAssetMenu(fileName = nameof(CharacterStateMachineDto),
        menuName = "State/" + nameof(CharacterStateMachineDto))]
    public class CharacterStateMachineDto : ScriptableObject
    {
        [Header("첫번째 state가 캐릭터의 처음 상태")] [SerializeField]
        private List<CharacterBaseStateDto> states;

        public StateMachine Build(ICharacterFsmController fsmController, AnimatorSystem animatorSystem, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            var stateMachine = new StateMachine();

            foreach (var stateData in states.ToArray())
            {
                var state = stateData.BuildState(stateMachine, fsmController, animatorSystem, animationParameter);
                
                stateMachine.TryAddState(state.GetStateType(), state);
            }

            return stateMachine;
        }
    }
}