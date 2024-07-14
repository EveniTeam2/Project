using UnityEngine;
using Unit.Stages.Creatures.Characters;
using System;
using Unit.Stages.Creatures.FSM.ActOnInput;

namespace Unit.Stages.Creatures {
    /// <summary>
    /// 여러 act를 묶어서 순차적으로 실행하는 클래스입니다.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = nameof(CompositeActCharacter), menuName = "Input/" + nameof(CompositeActCharacter))]
    public class CompositeActCharacter : ActCharacter {
        [SerializeField] private ActCharacter[] compositeActs;
        public override void Act(ActOnInput inputData, PlayerCreature character, int count) {
            foreach (var act in compositeActs) {
                act.Act(inputData, character, count);
            }
        }
    }
}