using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts.Creature.Data.MonsterData
{
    [CreateAssetMenu(fileName = nameof(MonsterDataSo), menuName = nameof(CreatureDataSo) + "/" + nameof(MonsterDataSo))]
    public class MonsterDataSo : CreatureDataSo
    {
        [Header("클래스 타입")]
        public MonsterType type;
    }
}