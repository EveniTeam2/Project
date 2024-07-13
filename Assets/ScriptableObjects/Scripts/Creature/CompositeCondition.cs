using Unit.Stages.Creatures;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(CompositeCondition), menuName = "State/" + nameof(CompositeCondition))]
    public class CompositeCondition : Condition {
        [SerializeField] Condition condition;
        [SerializeField] Condition compositeCondition;

        public override bool CheckCondition(BaseCreature target) {
            var ret = condition.CheckCondition(target);
            if (compositeCondition != null)
                ret = ret && compositeCondition.CheckCondition(target);
            return ret;
        }
    }
}