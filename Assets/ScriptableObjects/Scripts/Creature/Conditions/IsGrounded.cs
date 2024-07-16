using Unit.GameScene.Stages.Creautres;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(IsGrounded), menuName = "State/Condition/" + nameof(IsGrounded))]
    public class IsGrounded : Condition
    {
        public override bool CheckCondition(BaseCreature target)
        {
            return target.Movement.IsJump;
        }

        public override Condition GetCopy()
        {
            var copy = CreateInstance<IsGrounded>();

            return copy;
        }
    }
}