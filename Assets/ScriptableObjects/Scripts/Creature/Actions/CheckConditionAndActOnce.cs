using ScriptableObjects.Scripts.Creature.Conditions;
using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    //[CreateAssetMenu(fileName = nameof(CheckConditionAndActOnce),
    //    menuName = "State/Act/" + nameof(CheckConditionAndActOnce))]
    //public class CheckConditionAndActOnce : ActionData
    //{
    //    [SerializeField] private Condition _condition;
    //    [SerializeField] private ActionData _action;

    //    public override IStateAction GetStateAction()
    //    {
    //        var result = new StateActionCheckConditionAndActOnce(_condition, _action);
    //        return result;
    //    }

    //    public override IStateAction GetStateAction(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator) {
    //        throw new System.NotImplementedException();
    //    }
    //}

    //public class StateActionCheckConditionAndActOnce : IStateAction {
    //    private IStateCondition _condition;
    //    private IStateAction _action;
    //    private bool _isPerformed;

    //    public StateActionCheckConditionAndActOnce(IStateCondition condition, IStateAction action) {
    //        _condition = condition;
    //        _action = action;
    //    }

    //    public void OnAct(StateType stateName, int parameterHash) {
    //        if (!_isPerformed)
    //            if (_condition.CheckCondition()) {
    //                _action.OnAct(stateName, parameterHash);
    //                _isPerformed = true;
    //            }
    //    }

    //    private ICreatureServiceProvider OnExitState(StateType stateName, int parameterHash, ICreatureServiceProvider service) {
    //        service.UnregistStateEvent(stateName, EStateEventType.Exit, OnExitState);
    //        _isPerformed = false;
    //        return service;
    //    }
    //}
}