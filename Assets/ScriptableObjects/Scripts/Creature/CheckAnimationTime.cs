using Unit.Stages.Creatures;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(CheckAnimationTime), menuName = "State/Condition/" + nameof(CheckAnimationTime))]
    public class CheckAnimationTime : Condition {
        [SerializeField] protected float _percentage;
        public override bool CheckCondition(BaseCreature target) {
            var normalTime = target.HFSM.GetCurrentAnimationNormalizedTime();
            if (_percentage < normalTime)
                return true;
            return false;
        }
    }
}