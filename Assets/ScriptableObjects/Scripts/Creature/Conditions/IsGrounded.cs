using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(IsGrounded), menuName = "State/Condition/" + nameof(IsGrounded))]
    public class IsGrounded : Condition
    {
        [SerializeField] private bool isGrounded;
        public override IStateCondition GetStateCondition(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator) {
            return new StateConditionIsGrounded(isGrounded, movementSystem);
        }
    }

    public class StateConditionIsGrounded : IStateCondition {
        private readonly bool isGrounded;
        private readonly MovementSystem movement;

        public StateConditionIsGrounded(bool isGrounded, MovementSystem movement) {
            this.isGrounded = isGrounded;
            this.movement = movement;
        }

        public bool CheckCondition() {
            return !movement.IsInAir && isGrounded;
        }
    }
}