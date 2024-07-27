using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Data
{
    public class CharacterDataSo : CreatureDataSo
    {
        [Header("클래스 타입")]
        public CharacterClassType classType;
    }
}