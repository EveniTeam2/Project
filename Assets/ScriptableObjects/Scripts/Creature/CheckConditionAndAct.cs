using ScriptableObjects.Scripts.Creature;
using Unit.Stages.Creatures.Interfaces;
using UnityEngine;

namespace Assets.ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(CheckConditionAndAct), menuName = "State/Act/" + nameof(CheckConditionAndAct))]
    public class CheckConditionAndAct : ActionData {
        [SerializeField] Condition _condition;
        [SerializeField] ActionData _action;
        
        public override IState OnAct(IState state) {
            if (_condition.CheckCondition(state.StateMachine.Target))
                _action.OnAct(state);
            return state;
        }
    }
}
