using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(SetBoolParameter), menuName = "State/Act/" + nameof(SetBoolParameter))]
    public class SetBoolParameter : ActionData
    {
        [SerializeField] protected bool _value;

        public override IStateAction GetStateAction()
        {
            var ret = new StateActionSetBoolParameter(_value);
            return ret;
        }
    }

    public class StateActionSetBoolParameter : IStateAction {
        protected bool _value;

        public StateActionSetBoolParameter(bool value) {
            _value = value;
        }

        /// <summary>
        ///     상태 동작을 수행합니다.
        /// </summary>
        public IState OnAct(IState state) {
            state.StateMachine.SetBoolAnimator(state.ParameterHash, _value);
            return state;
        }
    }
}