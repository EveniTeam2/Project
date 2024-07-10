using System.Collections.Generic;
using UnityEngine;

namespace Unit.Character {
    [CreateAssetMenu(fileName = nameof(CharacterStateData), menuName = "State/" + nameof(CharacterStateData))]
    public class CharacterStateData : ScriptableObject {
        [SerializeField] private List<StateData> states;
        public List<StateData> StateDatas { get => states; }
    }
}