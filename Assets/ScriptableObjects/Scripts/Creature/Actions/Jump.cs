using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(Jump), menuName = "State/Act/" + nameof(Jump))]
    public class Jump : ActionData
    {
        [SerializeField] private float _jumpPower;

        public override IStateAction GetStateAction()
        {
            var result = new StateActionJump(_jumpPower);
            return result;
        }
    }

    public class StateActionJump : IStateAction {
        private float _jumpPower;

        public StateActionJump(float jumpPower) {
            _jumpPower = jumpPower;
        }

        public IState OnAct(IState state) {
            state.StateMachine.Target.Movement.SetJump(_jumpPower);
            return state;
        }
    }
}