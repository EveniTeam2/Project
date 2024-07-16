using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Stages.Creautres;
using Unit.GameScene.Stages.Creautres.Characters.Enums;
using Unit.GameScene.Stages.Creautres.FSM;
using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.FSM {
    public static class StateBuilder {
        public static StateMachine BuildState(BaseCreature target, StateDataDTO data) {
            var sm = new StateMachine(target);

            foreach (var stateData in data.StateDatas) {
                var state = BuildState(sm, stateData);
                sm.TryAddState(stateData.StateEnums, state);
            }

            return sm;
        }

        private static BaseState BuildState(StateMachine sm, StateData data) {
            Func<IState, IState> enter = data.OnEnter != null ? data.OnEnter.GetStateAction().OnAct : null;
            Func<IState, IState> exit = data.OnExit != null ? data.OnExit.GetStateAction().OnAct : null;
            Func<IState, IState> update = data.OnUpdate != null ? data.OnUpdate.GetStateAction().OnAct : null;
            Func<IState, IState> fixedUpdate = data.OnFixedUpdate != null
                ? data.OnFixedUpdate.GetStateAction().OnAct : null;
            Func<BaseCreature, bool> condition = data.Condition != null
                ? data.Condition.GetStateCondition().CheckCondition : null;

            if ((int)data.AnimParameterEnums >= Enum.GetValues(typeof(AnimationParameterEnums)).Length) {
                int hash = Animator.StringToHash(data.AnimParameterEnums.ToString());
                return new BaseState(data.StateEnums, hash, sm, enter, exit, update, fixedUpdate, condition);
            }

            return new BaseState(data.StateEnums, 0, sm, enter, exit, update, fixedUpdate, condition);
        }

        //private static SubState BuildSubState(StateMachine sm, StateData data) {
        //    Debug.Assert(data.States.Count > 0, "SubState에 최소한 1개의 BaseState가 있어야 합니다.");

        //    Action<IState> enter = null;
        //    if (data.OnEnter != null)
        //        enter = data.OnEnter.OnAct;
        //    Action<IState> exit = null;
        //    if (data.OnExit != null)
        //        exit = data.OnExit.OnAct;
        //    Action<IState> update = null;
        //    if (data.OnUpdate != null)
        //        update = data.OnUpdate.OnAct;
        //    Action<IState> fixedupdate = null;
        //    if (data.OnFixedUpdate != null)
        //        fixedupdate = data.OnFixedUpdate.OnAct;
        //    Func<BaseCreature, bool> condition = null;
        //    if (data.Condition != null)
        //        condition = data.Condition.CheckCondition;

        //    int hash = 0;
        //    if (data.AnimParameter.Length > 0) {
        //        hash = Animator.StringToHash(data.AnimParameter);
        //    }
        //    var sub = new SubState(sm, BuildState(sm, data.States[0]), data.StateName, hash, enter, exit, update, fixedupdate, condition);

        //    for (int i = 1; i < data.States.Count; ++i) {
        //        sub.TryAddState(data.States[i].StateName, BuildState(sm, data.States[i]));
        //    }
        //    return sub;
        //}
    }
}