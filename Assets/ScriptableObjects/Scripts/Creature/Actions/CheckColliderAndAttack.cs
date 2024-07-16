using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(CheckColliderAndAttack),
        menuName = "State/Act/" + nameof(CheckColliderAndAttack))]
    public class CheckColliderAndAttack : ActionData
    {
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private Vector2 direction;
        [SerializeField] private float _distance;
        [SerializeField] private int _attackMultiplier;

        public override IStateAction GetStateAction()
        {
            var result = new StateActionCheckColliderAndAttack(targetLayer, direction, _distance, _attackMultiplier);
            return result;
        }
    }

    public class StateActionCheckColliderAndAttack : IStateAction {
        private LayerMask targetLayer;
        private Vector2 direction;
        private float _distance;
        private int _attackMultiplier;
        public StateActionCheckColliderAndAttack(LayerMask targetLayer, Vector2 direction, float distance, int attackMultiplier) {
            this.targetLayer = targetLayer;
            this.direction = direction;
            _distance = distance;
            _attackMultiplier = attackMultiplier;
        }
        public IState OnAct(IState state) {
            if (state.StateMachine.Target.Battle.CheckCollider(targetLayer, direction, _distance, out var collider))
                foreach (var col in collider)
                    state.StateMachine.Target.Battle.Attack(col);
            return state;
        }
    }
}