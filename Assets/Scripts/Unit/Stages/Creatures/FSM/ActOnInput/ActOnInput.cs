using UnityEngine;
using ScriptableObjects.Scripts.Blocks;
using Unit.Stages.Creatures.Characters;

namespace Unit.Stages.Creatures {
    /// <summary>
    /// 입력과 행동을 연결하는 클래스입니다.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ActOnInput), menuName = "Input/" + nameof(ActOnInput))]
    public class ActOnInput : ScriptableObject {
        [SerializeField] BlockType inputType;
        [SerializeField] private string _stateName;
        [SerializeField] private ActCharacter act;

        public string StateName { get => _stateName; }
        public BlockType InputType { get => inputType; }
        public void Act(Character character, int count) {
            act.Act(this, character, count);
        }
    }
}