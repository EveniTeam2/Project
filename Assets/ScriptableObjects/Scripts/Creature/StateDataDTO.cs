using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature
{
    [CreateAssetMenu(fileName = nameof(StateDataDTO), menuName = "State/" + nameof(StateDataDTO))]
    public class StateDataDTO : ScriptableObject
    {
        [Header("첫번째 state가 캐릭터의 처음 상태")]
        [SerializeField] private List<StateData> states;
        public List<StateData> StateDatas => states;
    }
}