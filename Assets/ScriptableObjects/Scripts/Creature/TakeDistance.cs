using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature {
    public class TakeDistance : ActionData {
        [SerializeField] Vector2 direction;
        [SerializeField] float distance;
        [SerializeField] LayerMask targetLayer;
        public override IState OnAct(IState state) {
            var obj = Physics2D.Raycast(state.StateMachine.Target.transform.position, direction, distance, targetLayer);
            
            return state;
        }
    }
}