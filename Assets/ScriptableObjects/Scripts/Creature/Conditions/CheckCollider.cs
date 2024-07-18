using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(CheckCollider),
        menuName = "State/" + nameof(Condition) + "/" + nameof(CheckCollider))]
    public class CheckCollider : Condition
    {
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private Vector2 direction;
        [SerializeField] private float distance;
        [SerializeField] private bool yesOrNo;

        public override IStateCondition GetStateCondition(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator) {
            return new StateConditionCheckCollider(targetLayer, direction, distance, battleSystem, yesOrNo);
        }
    }

    public class StateConditionCheckCollider : IStateCondition {
        private LayerMask targetLayer;
        private Vector2 direction;
        private float distance;
        private readonly BattleSystem battleSystem;
        private bool isThereCollider;

        public StateConditionCheckCollider(LayerMask targetLayer, Vector2 direction, float distance, BattleSystem battleSystem, bool yesOrNo) {
            this.targetLayer = targetLayer;
            this.direction = direction;
            this.distance = distance;
            this.battleSystem = battleSystem;
        }

        public bool CheckCondition() {
            if (battleSystem.CheckCollider(targetLayer, direction, distance, out var collider))
                return isThereCollider == (collider.Length > 0);

            return isThereCollider == false;
        }
    }
}