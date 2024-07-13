using UnityEngine;

namespace ScriptableObjects.Scripts.Creature
{
    [CreateAssetMenu(fileName = nameof(StateData), menuName = "State/" + nameof(StateData))]
    public class StateData : ScriptableObject
    {
        [SerializeField] private string stateName;
        [SerializeField] private string animParameter;
        [SerializeField] private ActionData onEnter;
        [SerializeField] private ActionData onExit;
        [SerializeField] private ActionData onUpdate;
        [SerializeField] private ActionData onFixedUpdate;
        [Header("SubState이면 필요없고, BaseState인 경우에 없으면 무조건 가능한 것으로 간주함.")]
        [SerializeField] private Condition canTransitionToThis;

        public string StateName => stateName;
        public Condition Condition => canTransitionToThis;
        public ActionData OnEnter => onEnter;
        public ActionData OnExit => onExit;
        public ActionData OnUpdate => onUpdate;
        public ActionData OnFixedUpdate => onFixedUpdate;
        public string AnimParameter => animParameter;
    }
}