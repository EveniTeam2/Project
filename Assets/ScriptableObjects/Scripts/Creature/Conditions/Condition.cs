using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    public abstract class Condition : ScriptableObject
    {
        public abstract IStateCondition GetStateCondition(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator);
    }

    public interface IStateCondition {
        bool CheckCondition();
    }
}