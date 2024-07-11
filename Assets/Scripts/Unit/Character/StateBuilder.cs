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
        public abstract void OnAct(IState state);
    }

    public static class StateBuilder {
        public static StateMachine BuildState(BaseCharacter target, CharacterStateData data) {
            var sm = new StateMachine(target);

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

            Action<IState> enter = null;
            if (data.OnEnter != null)
                enter = data.OnEnter.OnAct;
            Action<IState> exit = null;
            if (data.OnExit != null)
                exit = data.OnExit.OnAct;
            Action<IState> update = null;
            if (data.OnUpdate != null)
                update = data.OnUpdate.OnAct;
            Action<IState> fixedupdate = null;
            if (data.OnFixedUpdate != null)
                fixedupdate = data.OnFixedUpdate.OnAct;
            Func<BaseCharacter, bool> condition = null;
            if (data.Condition != null)
                condition = data.Condition.CheckCondition;

            int hash = 0;
            if (data.AnimParameter.Length > 0) {
                hash = Animator.StringToHash(data.AnimParameter);
            }
            var sub = new SubState(sm, BuildState(sm, data.States[0]), data.StateName, hash, enter, exit, update, fixedupdate, condition);

            for (int i = 1; i < data.States.Count; ++i) {
                sub.TryAddState(data.States[i].StateName, BuildState(sm, data.States[i]));
            }
            return sub;
        }

        private static BaseState BuildState(StateMachine sm, StateData data) {
            Action<IState> enter = null;
            if (data.OnEnter != null)
                enter = data.OnEnter.OnAct;
            Action<IState> exit = null;
            if (data.OnExit != null)
                exit = data.OnExit.OnAct;
            Action<IState> update = null;
            if (data.OnUpdate != null)
                update = data.OnUpdate.OnAct;
            Action<IState> fixedupdate = null;
            if (data.OnFixedUpdate != null)
                fixedupdate = data.OnFixedUpdate.OnAct;
            Func<BaseCharacter, bool> condition = null;
            if (data.Condition != null)
                condition = data.Condition.CheckCondition;

            if (data.AnimParameter.Length > 0) {
                int hash = Animator.StringToHash(data.AnimParameter);
                return new BaseState(data.StateName, hash, sm, enter, exit, update, fixedupdate, condition);
            }
            return new BaseState(data.StateName, 0, sm, enter, exit, update, fixedupdate, condition);
        }
    }
}