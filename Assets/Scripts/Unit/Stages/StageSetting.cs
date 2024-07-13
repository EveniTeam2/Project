using System;
using Unit.Stages.Creatures;
using Unit.Stages.Creatures.Characters.Unit.Character;
using Unit.Stages.Creatures.Monsters;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Unit.Stages
{
    // TODO factory pattern을 사용하여 battle stage setting을 생성하여 넘겨줘야 한다.
    [Serializable]
    public struct StageSetting
    {
        // public AssetReference characterRef;
        public GameObject characterRef;
        public CharacterStat characterStat;
        public AssetReference[] monstersRef;
        public MonsterStat[] monsterStats;
        public BackgroundDisplay background;
        public Vector3 playerPosition;
        public Vector3 monsterSpawnOffset;
    }
}