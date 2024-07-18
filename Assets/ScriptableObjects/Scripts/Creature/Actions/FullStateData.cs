using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO {
    public abstract class FullStateData : ScriptableObject {
        abstract public FullState GetFullState(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine sm);
    }
}