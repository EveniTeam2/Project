using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    public abstract class Condition : ScriptableObject
    {
        public abstract IStateCondition GetStateCondition();
    }

    public interface IStateCondition {
        bool CheckCondition(Unit.GameScene.Stages.Creatures.Creature target);
    }
}