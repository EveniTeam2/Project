using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(Jump), menuName = "State/Act/" + nameof(Jump))]
    public class Jump : ActionData
    {
        [SerializeField] private float _jumpPower;

        public override IState OnAct(IState state)
        {
            state.StateMachine.Target.Movement.SetJump(_jumpPower);
            return state;
        }

        public override ActionData GetCopy()
        {
            var copy = CreateInstance<Jump>();
            copy._jumpPower = _jumpPower;
            return copy;
        }
    }
}