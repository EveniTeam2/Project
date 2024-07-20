using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(CheckAnimationTime), menuName = "State/" + nameof(Condition) + "/" + nameof(CheckAnimationTime))]
    public class CheckAnimationTime : Condition
    {
        [SerializeField] protected float percentage;
        [SerializeField] protected AnimationParameterEnums currentState;

        public override IStateCondition GetStateCondition(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator)
        {
            return new StateConditionCheckAnimationTime(percentage, currentState, animator);
        }
    }

    public class StateConditionCheckAnimationTime : IStateCondition {
        private readonly float _percentage;
        private readonly Animator _animator;
        private readonly AnimationParameterEnums _currentState;

        public StateConditionCheckAnimationTime(float percentage, AnimationParameterEnums currentState, Animator animator) {
            _percentage = percentage;
            _animator = animator;
            _currentState = currentState;
        }

        public bool CheckCondition()
        {
            var currentState = _animator.GetCurrentAnimatorStateInfo(0);

            if (currentState.tagHash == Animator.StringToHash(_currentState.ToString()))
            {
                var normalTime = currentState.normalizedTime;

                if (_percentage < normalTime)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}