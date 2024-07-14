using Unit.Boards.Blocks.Enums;
using Unit.Stages.Creatures.Characters;
using UnityEngine;

namespace Unit.Stages.Creatures.FSM.ActOnInput {
    /// <summary>
    /// 입력과 행동을 연결하는 클래스입니다.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ActOnInput), menuName = "Input/" + nameof(ActOnInput))]
    public class ActOnInput : ScriptableObject {
        [SerializeField] BlockType blockType;
        [SerializeField] private string _stateName;
        [SerializeField] private ActCharacter act;

        public string StateName { get => _stateName; }
        public BlockType BlockType { get => blockType; }
        public void Act(PlayerCreature character, int count) {
            act.Act(this, character, count);
        }
    }
}