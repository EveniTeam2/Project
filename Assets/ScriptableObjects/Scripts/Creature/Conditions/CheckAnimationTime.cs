using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions {
    [CreateAssetMenu(fileName = nameof(CheckAnimationTime), menuName = "State/" + nameof(Condition) + "/" + nameof(CheckAnimationTime))]
    public class CheckAnimationTime : Condition {
        [SerializeField] protected float _percentage;

        public override IStateCondition GetStateCondition(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator) {
            return new StateConditionCheckAnimationTime(_percentage, animator);
        }
    }

    public class StateConditionCheckAnimationTime : IStateCondition {
        protected float _percentage;
        private readonly Animator _animator;

        public StateConditionCheckAnimationTime(float percentage, Animator animator) {
            _percentage = percentage;
            this._animator = animator;
        }

        public bool CheckCondition() {
            var normalTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (_percentage < normalTime)
                return true;
            return false;
        }
    }
}