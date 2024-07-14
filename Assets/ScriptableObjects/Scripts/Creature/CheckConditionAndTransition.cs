using ScriptableObjects.Scripts.Creature;
using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;

namespace Assets.ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(CheckConditionAndTransition), menuName = "State/Act/" + nameof(CheckConditionAndTransition))]
    public class CheckConditionAndTransition : ActionData {
        [SerializeField] Condition _condition;
        [SerializeField] string _targetStateName;
        public override IState OnAct(IState state) {
            if (_condition.CheckCondition(state.StateMachine.Target))
                state.StateMachine.TryChangeState(_targetStateName);
            return state;
        }
    }
}
