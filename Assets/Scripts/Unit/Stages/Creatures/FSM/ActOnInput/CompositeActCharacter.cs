using UnityEngine;
using Unit.Stages.Creatures.Characters;

namespace Unit.Stages.Creatures {
    /// <summary>
    /// 여러 act를 묶어서 순차적으로 실행하는 클래스입니다.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(CompositeActCharacter), menuName = "SO/" + nameof(CompositeActCharacter))]
    public class CompositeActCharacter : ActCharacter {
        [SerializeField] private ActCharacter[] compositeActs;
        public override void Act(ActOnInput inputData, Character character, int count) {
            foreach (var act in compositeActs) {
                act.Act(inputData, character, count);
            }
        }
    }
}