using ScriptableObjects.Scripts.Creature;
using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace Assets.ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(Jump), menuName = "State/Act/" + nameof(Jump))]
    public class Jump : ActionData {
        [SerializeField] float _jumpPower;
        public override IState OnAct(IState state) {
            state.StateMachine.Target.Movement.SetJump(_jumpPower);
            return state;
        }
    }
}
