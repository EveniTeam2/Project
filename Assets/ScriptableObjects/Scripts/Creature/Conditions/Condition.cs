using Unit.GameScene.Stages.Creatures;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions {
    public abstract class Condition : ScriptableObject {
        /// <summary>
        /// 조건을 확인합니다.
        /// </summary>
        public abstract bool CheckCondition(BaseCreature target);
    }
}