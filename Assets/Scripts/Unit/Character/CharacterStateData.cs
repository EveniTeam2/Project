using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Character {
    [CreateAssetMenu(fileName = nameof(CharacterStateData), menuName = "SO/" + nameof(CharacterStateData))]
    public class CharacterStateData : ScriptableObject {
        [SerializeField] private List<StateData> states;
        public List<StateData> States { get => states; }
    }

    [CreateAssetMenu(fileName = nameof(StateData), menuName = "SO/" + nameof(StateData))]

    public class StateData : ScriptableObject {
        [SerializeField] private string Name;
        [SerializeField] private Condition Condition;
        [SerializeField] private ActionData OnEnter;
        [SerializeField] private ActionData OnExit;
        [SerializeField] private ActionData OnUpdate;
        [SerializeField] private ActionData OnFixedUpdate;
        [SerializeField] private List<StateData> states;
    }

    public abstract class Condition : ScriptableObject {
        public abstract bool CheckCondition(StateMachine sm);
    }

    public abstract class ActionData : ScriptableObject {
        public abstract void OnAct();
    }

    public static class StateBuilder {
        public static StateMachine BuildState(StateMachine sm, CharacterStateData data) {
            // TODO

            return sm;
        }
    }
}