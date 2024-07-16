using Unit.GameScene.Stages.Creautres;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(CheckCollider),
        menuName = "State/" + nameof(Condition) + "/" + nameof(CheckCollider))]
    public class CheckCollider : Condition
    {
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private Vector2 direction;
        [SerializeField] private float distance;

        public override bool CheckCondition(BaseCreature target)
        {
            if (target.Battle.CheckCollider(targetLayer, direction, distance, out var collider))
                return collider.Length > 0;

            return false;
        }

        public override Condition GetCopy()
        {
            var copy = CreateInstance<CheckCollider>();
            copy.targetLayer = targetLayer;
            copy.direction = direction;
            copy.distance = distance;
            return copy;
        }
    }
}