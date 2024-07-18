using System;
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
        public static StateMachine BuildState(StateDataDTO stateDataDto, Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator)
        {
            var stateMachine = new StateMachine(animator);

            foreach (var stateData in stateDataDto.StateDatas)
            {
                var state = BuildState(stateData, transform, battleSystem, healthSystem, movementSystem, animator, stateMachine);
                stateMachine.TryAddState(stateData.StateType, state);
            }

            return stateMachine;
        }

        private static BaseState BuildState(StateData data, Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine stateMachine)
        {
            Action<StateType, int> enter = data.OnEnter != null ? data.OnEnter.GetStateAction(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine).OnAct : null;
            Action<StateType, int> exit = data.OnExit != null ? data.OnExit.GetStateAction(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine).OnAct : null;
            Action<StateType, int> update = data.OnUpdate != null ? data.OnUpdate.GetStateAction(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine).OnAct : null;
            Action<StateType, int> fixedUpdate = data.OnFixedUpdate != null ? data.OnFixedUpdate.GetStateAction(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine).OnAct : null;
            
            Func<bool> condition = data.Condition != null ? data.Condition.GetStateCondition(transform, battleSystem, healthSystem, movementSystem, animator).CheckCondition : null;
            
            var fullState = data.OnEveryAction?.GetFullState(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine);
            var animationParameterHash = Animator.StringToHash(data.AnimParameterEnums.ToString());
            var baseState = new BaseState(data.StateType, animationParameterHash, enter, exit, update, fixedUpdate, condition);
            
            if (!ReferenceEquals(fullState, null))
            {
                var hash = Animator.StringToHash(data.AnimParameterEnums.ToString());
                var animationState = new BaseState(data.StateType, hash, enter, exit, update, fixedUpdate, condition);
                if (!ReferenceEquals(fullState, null))
                    fullState.SubscribeEvent(animationState);
                return animationState;
            }

            var state = new BaseState(data.StateType, 0, enter, exit, update, fixedUpdate, condition);
            if (!ReferenceEquals(fullState, null))
                fullState.SubscribeEvent(state);
            return state;
        }
    }
}