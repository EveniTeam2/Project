using ScriptableObjects.Scripts.Creature.Conditions;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(CheckConditionAndTransition),
        menuName = "State/Act/" + nameof(CheckConditionAndTransition))]
    public class CheckConditionAndTransition : ActionData
    {
        [SerializeField] private Condition condition;
        [SerializeField] private StateType targetStateName;

        public override IStateAction GetStateAction()
        {
            var result = new StateActionCheckConditionAndTransition(condition, targetStateName);
            return result;
        }
    }

    public class StateActionCheckConditionAndTransition : IStateAction {
        private IStateCondition condition;
        private StateType targetStateName;

        public StateActionCheckConditionAndTransition(Condition condition, StateType targetStateName) {
            this.condition = condition.GetStateCondition();
            this.targetStateName = targetStateName;
        }

        public IState OnAct(IState state) {
            if (condition.CheckCondition(state.StateMachine.Target))
                state.StateMachine.TryChangeState(targetStateName);

            return state;
        }
    }
}