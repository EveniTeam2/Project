using Core.Utils;
using System;
using Unit.Stages.Creatures.Monsters;
using UnityEngine;

namespace Unit.Stages {
    public class MonsterSpawnManager {
        [Serializable]
        public struct MonsterSpawnData {
            [Header("몬스터 오브젝트")]
            public MonsterCreature[] monstersRef;
            [Header("몬스터 스텟")]
            public MonsterStat[] monsterStats;
            [Header("몬스터 스폰 오프셋")]
            public Vector3 monsterSpawnOffset;
            [Header("몬스터 스폰 거리")]
            public float[] monsterSpawnPosition;
        }
        protected MonsterSpawnData _data;
        public MonsterSpawnManager(MonsterSpawnData data) {
            _data = data;
        }
        public void CreateMonster(MonsterCreature monster, CustomPool<MonsterCreature> pool) {
            
        }
        public void GetMonster(MonsterCreature monster) {

        }
        public void ReleaseMonster(MonsterCreature creature) {

        }
        public void DestoryMonster(MonsterCreature creature) {

        }
    }
}