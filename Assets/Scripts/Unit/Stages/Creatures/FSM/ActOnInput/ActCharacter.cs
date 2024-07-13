using UnityEngine;
using Unit.Stages.Creatures.Characters;

namespace Unit.Stages.Creatures {
    /// <summary>
    /// 행동을 똑같이 다루기 위한 추상 클래스입니다.
    /// </summary>
    public abstract class ActCharacter : ScriptableObject {
        public abstract void Act(ActOnInput inputData, PlayerCreature character, int count);
    }
}