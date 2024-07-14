using ScriptableObjects.Scripts.Creature;
using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace Assets.ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(CheckColliderAndAttack), menuName = "State/Act/" + nameof(CheckColliderAndAttack))]
    public class CheckColliderAndAttack : ActionData {
        [SerializeField] LayerMask targetLayer;
        [SerializeField] Vector2 direction;
        [SerializeField] float _distance;
        [SerializeField] int _attackMultiplier;
        public override IState OnAct(IState state) {
            if (state.StateMachine.Target.Battle.CheckCollider(targetLayer, direction, _distance, out var collidee)) {
                foreach (var col in collidee)
                    state.StateMachine.Target.Battle.Attack(col);
            }
            return state;
        }
    }
}
