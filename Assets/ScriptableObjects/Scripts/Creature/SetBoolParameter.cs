using Unit.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature
{
    [CreateAssetMenu(fileName = nameof(SetBoolParameter), menuName = "State/" + nameof(SetBoolParameter))]
    public class SetBoolParameter : ActionData
    {
        [SerializeField] protected bool _value;

        /// <summary>
        /// 상태 동작을 수행합니다.
        /// </summary>

        public override void OnAct(IState state)
        {
            state.StateMachine.SetBoolAnimator(state.ParameterHash, _value);
        }
    }
}