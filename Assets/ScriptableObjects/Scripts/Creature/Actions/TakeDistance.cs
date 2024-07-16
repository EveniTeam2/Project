using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    public class TakeDistance : ActionData
    {
        [SerializeField] private Vector2 direction;
        [SerializeField] private float distance;
        [SerializeField] private LayerMask targetLayer;

        public override IState OnAct(IState state)
        {
            var obj = Physics2D.Raycast(state.StateMachine.Target.transform.position, direction, distance, targetLayer);

            return state;
        }

        public override ActionData GetCopy()
        {
            var copy = CreateInstance<TakeDistance>();

            copy.direction = direction;
            copy.distance = distance;
            copy.targetLayer = targetLayer;

            return copy;
        }
    }
}