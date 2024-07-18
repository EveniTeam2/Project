using ScriptableObjects.Scripts.Creature.Conditions;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions {
    [CreateAssetMenu(fileName = nameof(CheckConditionAndTransition),
        menuName = "State/Act/" + nameof(CheckConditionAndTransition))]
    public class CheckConditionAndTransition : ActionData {
        [SerializeField] private Condition condition;
        [SerializeField] private StateType targetStateName;

        public override IStateAction GetStateAction(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine stateMachine) {
            return new StateActionCheckConditionAndTransition(condition.GetStateCondition(transform, battleSystem, healthSystem, movementSystem, animator), targetStateName, stateMachine);
        }
    }

    public class StateActionCheckConditionAndTransition : IStateAction {
        private IStateCondition condition;
        private StateType targetStateName;
        private readonly StateMachine sm;

        public StateActionCheckConditionAndTransition(IStateCondition condition, StateType targetStateName, StateMachine sm) {
            this.condition = condition;
            this.targetStateName = targetStateName;
            this.sm = sm;
        }

        public void OnAct(StateType stateName, int parameterHash) {
            if (condition.CheckCondition()) {
                sm.TryChangeState(targetStateName);
            }
        }
    }
}
