using Unit.GameScene.Stages.Creatures;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(IsGrounded), menuName = "State/Condition/" + nameof(IsGrounded))]
    public class IsGrounded : Condition
    {
        public override IStateCondition GetStateCondition()
        {
            var ret = new StateConditionIsGrounded();
            return ret;
        }
    }

    public class StateConditionIsGrounded : IStateCondition {
        public StateConditionIsGrounded() {
        }
        public bool CheckCondition(BaseCreature target) {
            return target.Movement.IsJump;
        }
    }
}