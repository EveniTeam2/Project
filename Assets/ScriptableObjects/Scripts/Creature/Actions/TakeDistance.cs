using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace ScriptableObjects.Scripts.Creature.Actions {
    public class TakeDistance : ActionData {
        [SerializeField] private Vector2 direction;
        [SerializeField] private float distance;
        [SerializeField] private LayerMask targetLayer;

        public override IStateAction GetStateAction() {
            var ret = new StateActionTakeDistance(direction, distance, targetLayer);
            return ret;
        }
    }

    public class StateActionTakeDistance : IStateAction {
        private Vector2 direction;
        private float distance;
        private LayerMask targetLayer;

        public StateActionTakeDistance(Vector2 direction, float distance, LayerMask targetLayer) {
            this.direction = direction;
            this.distance = distance;
            this.targetLayer = targetLayer;
        }

        public IState OnAct(IState state) {
            var obj = Physics2D.Raycast(state.StateMachine.Target.transform.position, direction, distance, targetLayer);
            if (obj.collider != null) {
                state.StateMachine.Target.Movement.SetBackStep(true);
            }
            return state;
        }
    }
}