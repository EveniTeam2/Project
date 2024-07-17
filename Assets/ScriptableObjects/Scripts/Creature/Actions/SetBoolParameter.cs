using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(SetBoolParameter), menuName = "State/Act/" + nameof(SetBoolParameter))]
    public class SetBoolParameter : ActionData {
        [SerializeField] protected bool _value;

        public override IStateAction GetStateAction(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator) {
            var ret = new StateActionSetBoolParameter(_value, animator);
            return ret;
        }
    }

    public class StateActionSetBoolParameter : IStateAction {
        protected bool _value;
        private readonly Animator _animator;

        public StateActionSetBoolParameter(bool value, Animator animator) {
            _value = value;
            this._animator = animator;
        }

        /// <summary>
        ///     상태 동작을 수행합니다.
        /// </summary>
        public void OnAct(StateType stateName, int parameterHash) {
            _animator.SetBool(parameterHash, _value);
        }
    }
}