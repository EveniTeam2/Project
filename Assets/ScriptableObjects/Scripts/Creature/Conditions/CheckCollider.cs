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

        public override IStateCondition GetStateCondition()
        {
            var ret = new StateConditionCheckCollider(targetLayer, direction, distance);
            return ret;
        }
    }

    public class StateConditionCheckCollider : IStateCondition {
        private LayerMask targetLayer;
        private Vector2 direction;
        private float distance;

        public StateConditionCheckCollider(LayerMask targetLayer, Vector2 direction, float distance) {
            this.targetLayer = targetLayer;
            this.direction = direction;
            this.distance = distance;
        }

        public bool CheckCondition(BaseCreature target) {
            if (target.Battle.CheckCollider(targetLayer, direction, distance, out var collider))
                return collider.Length > 0;

            return false;
        }
    }
}