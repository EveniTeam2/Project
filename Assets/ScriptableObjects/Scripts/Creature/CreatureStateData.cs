using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature
{
    [CreateAssetMenu(fileName = nameof(CreatureStateData), menuName = "State/" + nameof(CreatureStateData))]
    public class CreatureStateData : ScriptableObject
    {
        [SerializeField] private List<StateData> states;
        public List<StateData> StateDatas => states;
    }
}