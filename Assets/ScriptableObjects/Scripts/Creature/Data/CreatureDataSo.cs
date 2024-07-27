using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Data
{
    public class CreatureDataSo : ScriptableObject
    {
        [Header("프리팹"), Space(5)] public Unit.GameScene.Units.Creatures.Creature creature;
    }
}