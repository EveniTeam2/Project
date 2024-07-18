using ScriptableObjects.Scripts.Creature.Actions;
using ScriptableObjects.Scripts.Creature.Conditions;
using System;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;

namespace ScriptableObjects.Scripts.Creature.DTO {
    public class ActOneFixedUpdate : FullState {
        IStateCondition _condition;
        IStateAction _action;
        bool _isPerformed;
        public ActOneFixedUpdate(IStateAction action, IStateCondition condition) {
            _action = action;
            _condition = condition;
        }

        public override void Enter(StateType state, int hash) {
            _isPerformed = false;
        }

        public override void Exit(StateType state, int hash) {
        }

        public override void Update(StateType state, int hash) {
        }

        public override void FixedUpdate(StateType state, int hash) {
            if (!_isPerformed && _condition.CheckCondition())
                _action.OnAct(state, hash);
        }

        public override void SubscribeEvent(IState state) {
            state.OnEnter += Enter;
            state._onFixedUpdate += FixedUpdate;
        }
    }
}