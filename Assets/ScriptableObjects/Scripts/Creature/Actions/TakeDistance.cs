using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions {
    public class TakeDistance : ActionData {
        [SerializeField] private Vector2 direction;
        [SerializeField] private float distance;
        [SerializeField] private LayerMask targetLayer;

        public override IStateAction GetStateAction(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine stateMachine) {
            var ret = new StateActionTakeDistance(transform, direction, distance, targetLayer, movementSystem);
            return ret;
        }
    }

    public class StateActionTakeDistance : IStateAction {
        private Transform _transform;
        private Vector2 _direction;
        private float _distance;
        private LayerMask _targetLayer;
        private MovementSystem movementSystem;

        public StateActionTakeDistance(Transform _transform, Vector2 direction, float distance, LayerMask targetLayer, MovementSystem movementSystem) {
            this._transform = _transform;
            this._direction = direction;
            this._distance = distance;
            _targetLayer = targetLayer;
            this.movementSystem = movementSystem;
        }

        public void OnAct(StateType stateName, int parameterHash) {
            RaycastHit2D obj = Physics2D.Raycast(_transform.position, _direction, _distance, _targetLayer);
            if (obj.collider != null) {
                movementSystem.SetBackward(true);
            }
        }
    }
}