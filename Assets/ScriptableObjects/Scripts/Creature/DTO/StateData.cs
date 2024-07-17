using ScriptableObjects.Scripts.Creature.Actions;
using ScriptableObjects.Scripts.Creature.Conditions;
using System;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts.Creature.DTO {
    [CreateAssetMenu(fileName = nameof(StateData), menuName = "State/" + nameof(StateData))]
    public class StateData : ScriptableObject
    {
        [SerializeField] private StateType stateType;
        [SerializeField] private AnimationParameterEnums animParameterEnums;
        [SerializeField] private ActionData onEnter;
        [SerializeField] private ActionData onExit;
        [SerializeField] private ActionData onUpdate;
        [SerializeField] private ActionData onFixedUpdate;
        [SerializeField] private Condition canTransitionToThis;
        [SerializeField] private FullStateData onEveryAction;

        public StateType StateType => stateType;
        public AnimationParameterEnums AnimParameterEnums => animParameterEnums;
        public ActionData OnEnter => onEnter;
        public ActionData OnExit => onExit;
        public ActionData OnUpdate => onUpdate;
        public ActionData OnFixedUpdate => onFixedUpdate;
        public Condition Condition => canTransitionToThis;

        public FullStateData OnEveryAction { get => onEveryAction;}
    }
}