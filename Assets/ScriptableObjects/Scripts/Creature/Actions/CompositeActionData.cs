using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(CompositeActionData), menuName = "State/Act/" + nameof(CompositeActionData))]
    public class CompositeActionData : ActionData
    {
        [SerializeField] private ActionData[] actionDatas;

        public override IStateAction GetStateAction(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine stateMachine) {
            List<IStateAction> actions = new List<IStateAction>();
            foreach (var actionData in actionDatas) {
                actions.Add(actionData.GetStateAction(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine));
            }
            return new StateActionCompositeAction(actions.ToArray());
        }
    }

    public class StateActionCompositeAction : IStateAction {
        private IStateAction[] actions;

        public StateActionCompositeAction(IStateAction[] stateActions) {
            actions = stateActions;
        }

        public void OnAct(StateType stateName, int parameterHash) {
            foreach (var action in actions) {
                action.OnAct(stateName, parameterHash);
            }
        }
    }
}