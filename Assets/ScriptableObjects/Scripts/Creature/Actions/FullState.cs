using System;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;

namespace ScriptableObjects.Scripts.Creature.DTO {
    public abstract class FullState {
        public abstract void Enter(StateType state, int hash);
        public abstract void Exit(StateType state, int hash);
        public abstract void Update(StateType state, int hash);
        public abstract void FixedUpdate(StateType state, int hash);
        public abstract void SubscribeEvent(IState state);
    }
}