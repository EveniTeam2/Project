using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(MonsterStateMachineDTO), menuName = "State/" + nameof(MonsterStateMachineDTO))]
    public class MonsterStateMachineDTO : ScriptableObject
    {
        [Header("첫번째 state가 캐릭터의 처음 상태")]
        [SerializeField] private List<MonsterBaseStateDTO> states;
        public MonsterBaseStateDTO[] StateDatas => states.ToArray();

        public StateMachine BuildMonster(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, Dictionary<AnimationParameterEnums, int> animationParameter, MonsterEventPublisher eventPublisher)
        {
            var stateMachine = new StateMachine(animator);
            foreach (var stateData in StateDatas)
            {
                var state = stateData.BuildMonster(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine, animationParameter, eventPublisher);
                stateMachine.TryAddState(state.GetStateType(), state);
            }

            return stateMachine;
        }
    }
}