using ScriptableObjects.Scripts.Creature;
using Unit.Stages.Creatures.Interfaces;
using UnityEngine;

namespace Assets.ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(CheckConditionAndTransition), menuName = "State/Act/" + nameof(CheckConditionAndTransition))]
    public class CheckConditionAndTransition : ActionData {
        [SerializeField] Condition condition;
        [SerializeField] string targetStateName;
        public override IState OnAct(IState state) {
            if (condition.CheckCondition(state.StateMachine.Target))
            state.StateMachine.TryChangeState(targetStateName);
            return state;
        }
    }
}
