using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEditor.Animations;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    public static class StateBuilder
    {
        public static StateMachine BuildState(StateDataDTO stateDataDto, Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            var stateMachine = new StateMachine(animator);

            foreach (var stateData in stateDataDto.StateDatas)
            {
                var state = BuildState(stateData, transform, battleSystem, healthSystem, movementSystem, animator, stateMachine, animationParameter);
                stateMachine.TryAddState(stateData.StateType, state);
            }

            return stateMachine;
        }

        private static BaseState BuildState(StateData data, Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            Action<StateType, int> enter = data.OnEnter != null ? data.OnEnter.GetStateAction(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine).OnAct : null;
            Action<StateType, int> exit = data.OnExit != null ? data.OnExit.GetStateAction(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine).OnAct : null;
            Action<StateType, int> update = data.OnUpdate != null ? data.OnUpdate.GetStateAction(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine).OnAct : null;
            Action<StateType, int> fixedUpdate = data.OnFixedUpdate != null ? data.OnFixedUpdate.GetStateAction(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine).OnAct : null;
            
            Func<bool> condition = data.Condition != null ? data.Condition.GetStateCondition(transform, battleSystem, healthSystem, movementSystem, animator).CheckCondition : null;
            
            var fullState = data.OnEveryAction?.GetFullState(transform, battleSystem, healthSystem, movementSystem,
                animator, stateMachine);
            var baseState = new BaseState(data.StateType, animationParameter[data.AnimParameterEnums], enter, exit, update, fixedUpdate, condition);

            //if (!ReferenceEquals(fullState, null))
            //{
            //    //var hash = Animator.StringToHash(data.AnimParameterEnums.ToString());
            //    //var animationState = new BaseState(data.StateType, (int)data.AnimParameterEnums, enter, exit, update, fixedUpdate, condition);
            //    if (!ReferenceEquals(fullState, null))
            //        fullState.SubscribeEvent(baseState);
            //    return baseState;
            //}

            //var state = new BaseState(data.StateType, 0, enter, exit, update, fixedUpdate, condition);
            fullState?.SubscribeEvent(baseState);

            return baseState;
        }
    }
}