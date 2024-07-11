using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Character {

    // TODO condition 상속해서 각각 조건 만들기
    public abstract class Condition : ScriptableObject {
        public abstract bool CheckCondition(BaseCharacter target);
    }

    // TODO Action 상속해서 각각 동작 만들기
    public abstract class ActionData : ScriptableObject {
        public abstract void OnAct();
    }

    public static class StateBuilder {
        public static StateMachine BuildState(StateMachine sm, CharacterStateData data) {
            sm.Clear();

            foreach (var stateData in data.StateDatas) {
                if (stateData.States.Count > 0) {
                    var sub = BuildSubState(sm, stateData);
                    sm.TryAddState(stateData.StateName, sub);
                }
                else {
                    var state = BuildState(sm, stateData);
                    sm.TryAddState(stateData.StateName, state);
                }
            }
            return sm;
        }

        private static SubState BuildSubState(StateMachine sm, StateData data) {
            Debug.Assert(data.States.Count > 0, "SubState에 최소한 1개의 BaseState가 있어야 합니다.");

            var sub = new SubState(sm, data.StateName, BuildState(sm, data.States[0]), data.Condition.CheckCondition);
            for (int i = 1; i < data.States.Count; ++i) {
                sub.TryAddState(data.States[i].StateName, BuildState(sm, data.States[i]));
            }
            return sub;
        }

        private static BaseState BuildState(StateMachine sm, StateData data) {
            if (data.AnimParameter.Length > 0) {
                int hash = Animator.StringToHash(data.AnimParameter);
                return new BaseState(data.StateName, hash, sm, data.OnEnter.OnAct, data.OnExit.OnAct, data.OnUpdate.OnAct, data.OnFixedUpdate.OnAct, data.Condition.CheckCondition);
            }
            return new BaseState(data.StateName, 0, sm, data.OnEnter.OnAct, data.OnExit.OnAct, data.OnUpdate.OnAct, data.OnFixedUpdate.OnAct, data.Condition.CheckCondition);
        }
    }
}