using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts.Creature.Data
{
    public class CharacterDataSo : CreatureDataSo
    {
        [Header("클래스 타입")]
        public CharacterType type;
    }
}