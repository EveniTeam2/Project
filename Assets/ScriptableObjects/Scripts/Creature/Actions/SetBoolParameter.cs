using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(SetBoolParameter), menuName = "State/Act/" + nameof(SetBoolParameter))]
    public class SetBoolParameter : ActionData
    {
        [SerializeField] protected bool _value;

        /// <summary>
        ///     상태 동작을 수행합니다.
        /// </summary>
        public override IState OnAct(IState state)
        {
            state.StateMachine.SetBoolAnimator(state.ParameterHash, _value);
            return state;
        }

        public override ActionData GetCopy()
        {
            var copy = CreateInstance<SetBoolParameter>();
            copy._value = _value;
            return copy;
        }
    }
}