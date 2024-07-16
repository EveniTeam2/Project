using ScriptableObjects.Scripts.Creature.Conditions;
using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(CheckConditionAndActOnce),
        menuName = "State/Act/" + nameof(CheckConditionAndActOnce))]
    public class CheckConditionAndActOnce : ActionData
    {
        [SerializeField] private Condition _condition;
        [SerializeField] private ActionData _action;

        private bool _isPerformed;

        public override IState OnAct(IState state)
        {
            if (!_isPerformed)
                if (_condition.CheckCondition(state.StateMachine.Target))
                {
                    _action.OnAct(state);
                    _isPerformed = true;
                    state.OnExit += OnExitState;
                }

            return state;
        }

        private IState OnExitState(IState state)
        {
            state.OnExit -= OnExitState;
            _isPerformed = false;
            return state;
        }

        public override ActionData GetCopy()
        {
            var copy = CreateInstance<CheckConditionAndActOnce>();
            copy._condition = _condition?.GetCopy();
            copy._action = _action?.GetCopy();
            return copy;
        }
    }
}