using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(Jump), menuName = "State/Act/" + nameof(Jump))]
    public class Jump : ActionData
    {
        [SerializeField] private float _jumpPower;

        public override IStateAction GetStateAction(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine stateMachine) {
            var result = new StateActionJump(_jumpPower, movementSystem);
            return result;
        }
    }

    public class StateActionJump : IStateAction {
        private float _jumpPower;
        private readonly MovementSystem _movement;

        public StateActionJump(float jumpPower, MovementSystem movement) {
            _jumpPower = jumpPower;
            this._movement = movement;
        }

        public void OnAct(StateType stateName, int parameterHash) {
            _movement.Jump(_jumpPower);
        }
    }
}