using ScriptableObjects.Scripts.Creature.Conditions;
using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(CheckConditionAndActOnce),
        menuName = "State/Act/" + nameof(CheckConditionAndActOnce))]
    public class CheckConditionAndActOnce : ActionData
    {
        [SerializeField] private Condition _condition;
        [SerializeField] private ActionData _action;

        public override IStateAction GetStateAction()
        {
            var result = new StateActionCheckConditionAndActOnce(_condition, _action);
            return result;
        }
    }

    public class StateActionCheckConditionAndActOnce : IStateAction {
        private IStateCondition _condition;
        private IStateAction _action;
        private bool _isPerformed;

        public StateActionCheckConditionAndActOnce(Condition condition, ActionData action) {
            _condition = condition.GetStateCondition();
            _action = action.GetStateAction();
        }

        public IState OnAct(IState state) {
            if (!_isPerformed)
                if (_condition.CheckCondition(state.StateMachine.Target)) {
                    _action.OnAct(state);
                    _isPerformed = true;
                    state.OnExit += OnExitState;
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