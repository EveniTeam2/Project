using ScriptableObjects.Scripts.Creature.Actions;
using ScriptableObjects.Scripts.Creature.Conditions;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(StateData), menuName = "State/" + nameof(StateData))]
    public class StateData : ScriptableObject
    {
        [FormerlySerializedAs("stateEnums")] [SerializeField] private StateType stateType;
        [SerializeField] private AnimationParameterEnums animParameterEnums;
        [SerializeField] private ActionData onEnter;
        [SerializeField] private ActionData onExit;
        [SerializeField] private ActionData onUpdate;
        [SerializeField] private ActionData onFixedUpdate;
        [Header("SubState이면 필요없고, BaseState인 경우에 없으면 무조건 가능한 것으로 간주함.")]
        [SerializeField] private Condition canTransitionToThis;

        public StateType StateType => stateType;
        public AnimationParameterEnums AnimParameterEnums => animParameterEnums;
        public ActionData OnEnter => onEnter;
        public ActionData OnExit => onExit;
        public ActionData OnUpdate => onUpdate;
        public ActionData OnFixedUpdate => onFixedUpdate;
        public Condition Condition => canTransitionToThis;
    }
}