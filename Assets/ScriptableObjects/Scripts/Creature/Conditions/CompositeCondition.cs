using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(CompositeCondition), menuName = "State/Condition/" + nameof(CompositeCondition))]
    public class CompositeCondition : Condition
    {
        [SerializeField] private Condition[] conditions;

        public override IStateCondition GetStateCondition(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator) {
            List<IStateCondition> result = new List<IStateCondition>();
            foreach (var condition in conditions) {
                result.Add(condition.GetStateCondition(transform, battleSystem, healthSystem, movementSystem, animator));
            }
            return new StateConditionCompositeCondition(result.ToArray());
        }
    }

    public class StateConditionCompositeCondition : IStateCondition {
        private IStateCondition[] conditions;

        public StateConditionCompositeCondition(IStateCondition[] conditions) {
            this.conditions = conditions;
        }

        public bool CheckCondition() {
            for (var i = 0; i < conditions.Length; ++i)
                if (!conditions[i].CheckCondition())
                    return false;
            return true;
        }
    }
}