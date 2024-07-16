using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    public abstract class ActionData : ScriptableObject
    {
        /// <summary>
        /// 동작을 수행합니다.
        /// </summary>
        public abstract IState OnAct(IState state);

        public abstract ActionData GetCopy();
    }
}