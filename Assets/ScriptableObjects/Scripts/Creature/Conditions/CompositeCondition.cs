using Unit.GameScene.Stages.Creautres;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(CompositeCondition), menuName = "State/Condition/" + nameof(CompositeCondition))]
    public class CompositeCondition : Condition
    {
        [SerializeField] private Condition[] conditions;

        public override IStateCondition GetStateCondition()
        {
            var ret = new StateConditionCompositeCondition(conditions);
            return ret;
        }
    }

    public class StateConditionCompositeCondition : IStateCondition {
        private IStateCondition[] conditions;

        public StateConditionCompositeCondition(Condition[] conditions) {
            this.conditions = new IStateCondition[conditions.Length];
            for (int i = 0; i < conditions.Length; ++i) {
                this.conditions[i] = conditions[i]?.GetStateCondition();
            }
        }

        public bool CheckCondition(BaseCreature target) {
            for (var i = 0; i < conditions.Length; ++i)
                if (!conditions[i].CheckCondition(target))
                    return false;
            return true;
        }
    }
}