using UnityEngine;

namespace Unit.Character {
    [CreateAssetMenu(fileName = nameof(CheckAnimationTime), menuName = "State/" + nameof(CheckAnimationTime))]
    public class CheckAnimationTime : Condition {
        [SerializeField] protected float _percentage;
        public override bool CheckCondition(BaseCharacter target) {
            var normalTime = target.HFSM.GetCurrentAnimationNormalizedTime();
            if (_percentage < normalTime)
                return true;
            return false;
        }
    }
}