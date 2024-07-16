using ScriptableObjects.Scripts.Creature.Actions;
using ScriptableObjects.Scripts.Creature.Conditions;
using Unit.GameScene.Stages.Creautres.Characters.Enums;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(StateData), menuName = "State/" + nameof(StateData))]
    public class StateData : ScriptableObject
    {
        [SerializeField] private StateEnums stateEnums;
        [SerializeField] private AnimationParameterEnums animParameterEnums;
        [SerializeField] private ActionData onEnter;
        [SerializeField] private ActionData onExit;
        [SerializeField] private ActionData onUpdate;
        [SerializeField] private ActionData onFixedUpdate;
        [Header("SubState이면 필요없고, BaseState인 경우에 없으면 무조건 가능한 것으로 간주함.")]
        [SerializeField] private Condition canTransitionToThis;

        public StateEnums StateEnums => stateEnums;
        public Condition Condition => canTransitionToThis;
        public ActionData OnEnter => onEnter;
        public ActionData OnExit => onExit;
        public ActionData OnUpdate => onUpdate;
        public ActionData OnFixedUpdate => onFixedUpdate;
        public AnimationParameterEnums AnimParameterEnums => animParameterEnums;

        public StateData GetCopy() {
            var obj = CreateInstance<StateData>();
            obj.stateEnums = stateEnums;
            obj.animParameterEnums = animParameterEnums;
            obj.onEnter = onEnter?.GetCopy();
            obj.onExit = onExit?.GetCopy();
            obj.onUpdate = onUpdate?.GetCopy();
            obj.onFixedUpdate = onFixedUpdate?.GetCopy();
            obj.canTransitionToThis = canTransitionToThis?.GetCopy();
            return obj;
        }
    }
}