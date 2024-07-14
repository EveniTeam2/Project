using Unit.GameScene.Stages.Creatures.Characters;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.FSM.ActOnInput {
    /// <summary>
    /// 행동을 똑같이 다루기 위한 추상 클래스입니다.
    /// </summary>
    public abstract class ActCharacter : ScriptableObject {
        public abstract void Act(ActOnInput inputData, PlayerCreature character, int count);
    }
}