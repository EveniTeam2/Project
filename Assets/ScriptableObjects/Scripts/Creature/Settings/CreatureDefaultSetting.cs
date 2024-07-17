using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts.Creature.Settings
{
    public class CreatureDefaultSetting : ScriptableObject
    {
        [Header("프리팹"), Space(5)] public Unit.GameScene.Stages.Creatures.Creature baseCreature;
    }
}