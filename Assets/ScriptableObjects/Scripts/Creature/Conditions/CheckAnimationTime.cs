using Unit.GameScene.Stages.Creautres;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions {
    [CreateAssetMenu(fileName = nameof(CheckAnimationTime), menuName = "State/" + nameof(Condition) + "/" + nameof(CheckAnimationTime))]
    public class CheckAnimationTime : Condition {
        [SerializeField] protected float _percentage;

        public override IStateCondition GetStateCondition() {
            var ret = new StateConditionCheckAnimationTime(_percentage);
            return ret;
        }
    }

    public class StateConditionCheckAnimationTime : IStateCondition {
        protected float _percentage;

        public StateConditionCheckAnimationTime(float percentage) {
            _percentage = percentage;
        }

        public bool CheckCondition(BaseCreature target) {
            var normalTime = target.HFSM.GetCurrentAnimationNormalizedTime();
            if (_percentage < normalTime)
                return true;
            return false;
        }
    }
}