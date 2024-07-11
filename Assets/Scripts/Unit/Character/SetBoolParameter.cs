using UnityEngine;

namespace Unit.Character {
    [CreateAssetMenu(fileName = nameof(SetBoolParameter), menuName = "State/" + nameof(SetBoolParameter))]
    public class SetBoolParameter : ActionData {
        [SerializeField] protected bool _value;
        public override void OnAct(BaseState state) {
            state.StateMachine.SetBoolAnimator(state.ParameterHash, _value);
        }
    }
}