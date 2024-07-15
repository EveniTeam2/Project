using Unit.GameScene.Stages.Creatures.Interfaces;
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

        public override IState OnAct(IState state)
        {
            if (state.StateMachine.Target.Battle.CheckCollider(targetLayer, direction, _distance, out var collider))
                foreach (var col in collider)
                    state.StateMachine.Target.Battle.Attack(col);
            return state;
        }

        public override ActionData GetCopy()
        {
            var copy = CreateInstance<CheckColliderAndAttack>();
            copy.targetLayer = targetLayer;
            copy.direction = direction;
            copy._distance = _distance;
            copy._attackMultiplier = _attackMultiplier;
            return copy;
        }
    }
}