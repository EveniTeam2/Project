using System;
using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts.Creature.Data
{
    [Serializable]
    public class MonsterPrefabInfo
    {
        public MonsterType monsterType;
        public GameObject monsterPrefab;
    }
}