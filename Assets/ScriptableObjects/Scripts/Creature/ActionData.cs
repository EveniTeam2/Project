using Unit.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature {
    public abstract class ActionData : ScriptableObject
    {
        /// <summary>
        /// 동작을 수행합니다.
        /// </summary>
        public abstract IState OnAct(IState state);
    }

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