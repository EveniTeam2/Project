using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(MonsterStateMachineDTO), menuName = "State/" + nameof(MonsterStateMachineDTO))]
    public class MonsterStateMachineDTO : ScriptableObject
    {
        [Header("첫번째 state가 캐릭터의 처음 상태")]
        [SerializeField] private List<MonsterBaseStateDTO> states;
        public MonsterBaseStateDTO[] StateDatas => states.ToArray();

        public StateMachine BuildMonster(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, AnimatorEventReceiver animatorEventReceiver, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            var stateMachine = new StateMachine(animatorEventReceiver);
            foreach (var stateData in StateDatas)
            {
                var state = stateData.BuildMonster(transform, battleSystem, healthSystem, movementSystem, stateMachine, animatorEventReceiver, animationParameter);
                stateMachine.TryAddState(state.GetStateType(), state);
            }

            return stateMachine;
        }
    }
}