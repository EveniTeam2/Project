using System.Collections.Generic;
using UnityEngine;

namespace Unit.Character {
    [CreateAssetMenu(fileName = nameof(StateData), menuName = "State/" + nameof(StateData))]
    public class StateData : ScriptableObject {
        [SerializeField] private string stateName;
        [SerializeField] private string animParameter;
        [SerializeField] private ActionData onEnter;
        [SerializeField] private ActionData onExit;
        [SerializeField] private ActionData onUpdate;
        [SerializeField] private ActionData onFixedUpdate;
        [Header("SubState이면 필요없고, BaseState인 경우에 없으면 무조건 가능한 것으로 간주함.")]
        [SerializeField] private Condition canTransitionToThis;
        [Header("SubState로 만들려면 지정해야함.")]
        [SerializeField] private List<StateData> states;

        public string StateName { get => stateName; }
        public Condition Condition { get => canTransitionToThis; }
        public ActionData OnEnter { get => onEnter; }
        public ActionData OnExit { get => onExit; }
        public ActionData OnUpdate { get => onUpdate; }
        public ActionData OnFixedUpdate { get => onFixedUpdate; }
        public List<StateData> States { get => states; }
        public string AnimParameter { get => animParameter;}
    }
}