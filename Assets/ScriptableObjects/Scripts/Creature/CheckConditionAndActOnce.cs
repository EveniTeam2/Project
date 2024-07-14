using ScriptableObjects.Scripts.Creature;
using Unit.Stages.Creatures.Interfaces;
using UnityEngine;

namespace Assets.ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(CheckConditionAndActOnce), menuName = "State/Act/" + nameof(CheckConditionAndActOnce))]
    public class CheckConditionAndActOnce : ActionData {
        [SerializeField] Condition _condition;
        [SerializeField] ActionData _action;

        private bool _isPerformed = false;
        public override IState OnAct(IState state) {
            if (!_isPerformed) {
                if (_condition.CheckCondition(state.StateMachine.Target)) {
                    _action.OnAct(state);
                    _isPerformed = true;
                    state.OnExit += OnExitState;
                }
            }
            return state;
        }

        private IState OnExitState(IState state) {
            state.OnExit -= OnExitState;
            _isPerformed = false;
            return state;
        }
    }
}
