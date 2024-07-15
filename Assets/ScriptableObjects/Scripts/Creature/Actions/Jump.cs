using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions {
    [CreateAssetMenu(fileName = nameof(Jump), menuName = "State/Act/" + nameof(Jump))]
    public class Jump : ActionData {
        [SerializeField] float _jumpPower;
        public override IState OnAct(IState state) {
            state.StateMachine.Target.Movement.SetJump(_jumpPower);
            return state;
        }
    }
}
