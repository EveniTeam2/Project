using UnityEngine;

namespace Unit.Character {
    [CreateAssetMenu(fileName = nameof(CompositeCondition), menuName = "State/" + nameof(CompositeCondition))]
    public class CompositeCondition : Condition {
        [SerializeField] Condition condition;
        [SerializeField] CompositeCondition compositeCondition;

        public override bool CheckCondition(BaseCharacter target) {
            var ret = condition.CheckCondition(target);
            if (compositeCondition != null)
                ret = ret && compositeCondition.CheckCondition(target);
            return ret;
        }
    }
}