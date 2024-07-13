using UnityEngine;
using Unit.Stages.Creatures.Characters;

namespace Unit.Stages.Creatures {
    /// <summary>
    /// 행동을 똑같이 다루기 위한 추상 클래스입니다.
    /// </summary>
    public abstract class ActCharacter : ScriptableObject {
        public abstract void Act(Character character, int count);
    }

    public class CompositeActCharacter : ActCharacter {
        [SerializeField] private ActCharacter compositeAct;
        [SerializeField] private ActCharacter baseAct;
        public override void Act(Character character, int count) {
            compositeAct.Act(character, count);
            baseAct.Act(character, count);
        }
    }
}