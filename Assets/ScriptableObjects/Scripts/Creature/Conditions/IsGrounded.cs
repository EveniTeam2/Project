using Unit.GameScene.Stages.Creatures;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions {
    [CreateAssetMenu(fileName = nameof(IsGrounded), menuName = "State/Condition/" + nameof(IsGrounded))]
    public class IsGrounded : Condition {
        public override bool CheckCondition(BaseCreature target) {
            return target.Movement.IsJump;
        }
    }
}