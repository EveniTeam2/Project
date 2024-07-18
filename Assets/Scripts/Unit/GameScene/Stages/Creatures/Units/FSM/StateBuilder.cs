using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    public static class StateBuilder
    {
        public static StateMachine BuildState(StateDataDTO data, Transform tr, BattleSystem ba, HealthSystem he,
            MovementSystem mo, Animator an)
        {
            var sm = new StateMachine(an);

            foreach (var stateData in data.StateDatas)
            {
                var state = BuildState(stateData, tr, ba, he, mo, an, sm);
                sm.TryAddState(stateData.StateType, state);
            }

            return sm;
        }

        private static BaseState BuildState(StateData data, Transform tr, BattleSystem ba, HealthSystem he,
            MovementSystem mo, Animator an, StateMachine sm)
        {
            Action<StateType, int> enter =
                data.OnEnter != null ? data.OnEnter.GetStateAction(tr, ba, he, mo, an, sm).OnAct : null;
            Action<StateType, int> exit =
                data.OnExit != null ? data.OnExit.GetStateAction(tr, ba, he, mo, an, sm).OnAct : null;
            Action<StateType, int> update = data.OnUpdate != null
                ? data.OnUpdate.GetStateAction(tr, ba, he, mo, an, sm).OnAct
                : null;
            Action<StateType, int> fixedUpdate = data.OnFixedUpdate != null
                ? data.OnFixedUpdate.GetStateAction(tr, ba, he, mo, an, sm).OnAct
                : null;
            Func<bool> condition = data.Condition != null
                ? data.Condition.GetStateCondition(tr, ba, he, mo, an).CheckCondition
                : null;
            var full = data.OnEveryAction?.GetFullState(tr, ba, he, mo, an, sm) ?? null;

            if ((int)data.AnimParameterEnums >= Enum.GetValues(typeof(AnimationParameterEnums)).Length)
            {
                var hash = Animator.StringToHash(data.AnimParameterEnums.ToString());
                var animationState = new BaseState(data.StateType, hash, enter, exit, update, fixedUpdate, condition);
                if (!ReferenceEquals(full, null))
                    animationState.SetFullState(full);
                return animationState;
            }

            var state = new BaseState(data.StateType, 0, enter, exit, update, fixedUpdate, condition);
            if (!ReferenceEquals(full, null))
                state.SetFullState(full);
            return state;
        }
    }
}