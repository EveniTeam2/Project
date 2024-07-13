using Unit.Stages.Creatures;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(CompositeCondition), menuName = "State/" + nameof(CompositeCondition))]
    public class CompositeCondition : Condition {
        [SerializeField] Condition[] conditions;

        public override bool CheckCondition(BaseCreature target) {
            for (int i = 0; i < conditions.Length; ++i) {
                if (!conditions[i].CheckCondition(target))
                    return false;
            }
            return true;
        }
    }
}