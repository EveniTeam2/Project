using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(StateMachineDTO), menuName = "State/" + nameof(StateMachineDTO))]
    public class StateMachineDTO : ScriptableObject
    {
        [Header("첫번째 state가 캐릭터의 처음 상태")]
        [SerializeField] private List<BaseStateDTO> states;
        public BaseStateDTO[] StateDatas => states.ToArray();

        public StateMachine Build(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            var stateMachine = new StateMachine(animator);
            foreach (var stateData in StateDatas)
            {
                var state = stateData.Build(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine, animationParameter);
                stateMachine.TryAddState(state.GetStateType(), state);
            }

            return stateMachine;
        }
    }
}