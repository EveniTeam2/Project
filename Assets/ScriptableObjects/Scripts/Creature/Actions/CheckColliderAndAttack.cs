using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions {
    [CreateAssetMenu(fileName = nameof(CheckColliderAndAttack), menuName = "State/Act/" + nameof(CheckColliderAndAttack))]
    public class CheckColliderAndAttack : ActionData {
        [SerializeField] LayerMask targetLayer;
        [SerializeField] Vector2 direction;
        [SerializeField] float _distance;
        [SerializeField] int _attackMultiplier;
        public override IState OnAct(IState state) {
            if (state.StateMachine.Target.Battle.CheckCollider(targetLayer, direction, _distance, out var collider)) {
                foreach (var col in collider)
                    state.StateMachine.Target.Battle.Attack(col);
            }
            return state;
        }
    }
}
