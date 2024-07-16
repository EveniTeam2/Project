using Unit.GameScene.Boards.Blocks.Enums;
using Unit.GameScene.Stages.Creautres.Characters;
using UnityEngine;

namespace Unit.GameScene.Stages.Creautres.FSM.ActOnInput
{
    /// <summary>
    ///     입력과 행동을 연결하는 클래스입니다.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ActOnInput), menuName = "Input/" + nameof(ActOnInput))]
    public class ActOnInput : ScriptableObject
    {
        [SerializeField] private BlockType blockType;
        [SerializeField] private string _stateName;
        [SerializeField] private ActCharacter act;

        public string StateName => _stateName;
        public BlockType BlockType => blockType;

        public void Act(Character character, int count)
        {
            act.Act(this, character, count);
        }
    }
}