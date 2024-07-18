using ScriptableObjects.Scripts.Creature.Conditions;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(CheckConditionAndAct), menuName = "State/Act/" + nameof(CheckConditionAndAct))]
    public class CheckConditionAndAct : ActionData {
        [SerializeField] private Condition _condition;
        [SerializeField] private ActionData _action;

        public override IStateAction GetStateAction(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine stateMachine) {
            IStateCondition con = _condition.GetStateCondition(transform, battleSystem, healthSystem, movementSystem, animator);
            IStateAction act = _action.GetStateAction(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine);
            return new StateActionCheckConditionAndAct(con, act);
        }
    }

    public class StateActionCheckConditionAndAct : IStateAction {
        private IStateCondition _condition;
        private IStateAction _action;

        public StateActionCheckConditionAndAct(IStateCondition condition, IStateAction action) {
            _condition = condition;
            _action = action;
        }

        public void OnAct(StateType stateName, int parameterHash) {
            if (_condition.CheckCondition()) {
                _action.OnAct(stateName, parameterHash);
            }
        }
    }
}