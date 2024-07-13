using UnityEngine;
using ScriptableObjects.Scripts.Blocks;

namespace Unit.Stages.Creatures {
    /// <summary>
    /// 입력과 행동을 연결하는 클래스입니다.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ActOnInput), menuName = "SO/" + nameof(ActOnInput))]
    public class ActOnInput : ScriptableObject {
        [SerializeField] BlockType inputType;
        [SerializeField] private string _animParameter;
        [SerializeField] private ActCharacter act;

        public string AnimParameter { get => _animParameter; }
        public ActCharacter Act { get => act; }
        public BlockType InputType { get => inputType; }
    }
}