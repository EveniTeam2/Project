using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterStateMachineDTO), menuName = "State/" + nameof(MonsterStateMachineDTO))]
    public class MonsterStateMachineDTO : ScriptableObject
    {
        [Header("첫번째 state가 캐릭터의 처음 상태")]
        [SerializeField] private List<MonsterBaseStateDto> states;

        public StateMachine Build(Transform targetTransform, IMonsterFsmController fsmController, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            var stateMachine = new StateMachine();
            
            foreach (var stateData in states.ToArray())
            {
                var state = stateData.BuildState(targetTransform, stateMachine, animationParameter, fsmController);
                stateMachine.TryAddState(state.GetStateType(), state);
            }

            return stateMachine;
        }
    }
}