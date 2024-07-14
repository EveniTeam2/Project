using System;
using System.Collections.Generic;
using Unit.Stages.Creatures.Monsters;
using UnityEngine;

namespace Unit.Stages {
    public partial class MonsterSpawnManager {
        [Serializable]
        [CreateAssetMenu(fileName = nameof(MonsterSpawnData), menuName = "SO/" + nameof(MonsterSpawnData))]
        public class MonsterSpawnData : ScriptableObject {
            [Header("몬스터 오브젝트")]
            public MonsterCreature[] monstersRef;
            [Header("몬스터 스텟")]
            public MonsterStat[] monsterStats;
            [Header("몬스터 스폰 오프셋")]
            public Vector3 monsterSpawnOffset;
            [Header("몬스터 스폰 타입 오프셋")]
            public float monsterSpawnTimeOffset;
            [Header("랜덤 몬스터")]
            public RandomMonsterGroup[] monsterRandomGroup;
            [Header("확정 몬스터")]
            public StaticMonsterGroup monsterStaticGroup;

            
        }
    }
}