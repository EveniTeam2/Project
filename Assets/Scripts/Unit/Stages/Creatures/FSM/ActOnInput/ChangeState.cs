using UnityEngine;
using Unit.Stages.Creatures.Characters;

namespace Unit.Stages.Creatures {
    /// <summary>
    /// 입력에 따라 상태를 바꾸는 행동입니다.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ChangeState), menuName = "SO/" + nameof(ChangeState))]
    public class ChangeState : ActCharacter {
        public override void Act(ActOnInput inputData, Character character, int count) {
            character.HFSM.TryChangeState(inputData.StateName);
        }
    }
}