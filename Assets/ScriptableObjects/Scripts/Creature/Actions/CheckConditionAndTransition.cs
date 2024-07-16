using ScriptableObjects.Scripts.Creature.Conditions;
using Unit.GameScene.Stages.Creautres.Characters.Enums;
using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(CheckConditionAndTransition),
        menuName = "State/Act/" + nameof(CheckConditionAndTransition))]
    public class CheckConditionAndTransition : ActionData
    {
        [SerializeField] private Condition condition;
        [SerializeField] private StateEnums targetStateName;

        public override IState OnAct(IState state)
        {
            if (condition.CheckCondition(state.StateMachine.Target)) state.StateMachine.TryChangeState(targetStateName);

            return state;
        }

        public override ActionData GetCopy()
        {
            var copy = CreateInstance<CheckConditionAndTransition>();
            copy.condition = condition.GetCopy();
            copy.targetStateName = targetStateName;
            return copy;
        }
    }
}