using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    public abstract class ActionData : ScriptableObject
    {
        public abstract IStateAction GetStateAction();
    }

    public interface IStateAction {
        IState OnAct(IState state);
    }
}