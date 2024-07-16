using System;
using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Stages;
using Unit.GameScene.Stages.Creautres.Monsters;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers.Modules
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(MonsterSpawnData), menuName = "SO/" + nameof(MonsterSpawnData))]
    public class MonsterSpawnData : ScriptableObject
    {
        [Header("몬스터 오브젝트")] public Monster[] monstersRef;

        [Header("몬스터 스텟")] public MonsterStat[] monsterStats;

        [Header("몬스터 스폰 오프셋")] public Vector3 monsterSpawnOffset;

        [Header("몬스터 스폰 타입 오프셋")] public float monsterSpawnTimeOffset;

        [Header("몬스터 스폰에 대한 데이터")] public MonsterGroup[] monsterGroup;
    }

    /// <summary>
    ///     해당 거리에서 확정 소환 그룹 모음
    /// </summary>
    [Serializable]
    public struct MonsterGroup
    {
        public SpawnDecider spawnDecider;
        public List<SpawnGroup> monsterSpawnGroups;
        private int _totalWeight;

        public int TotalWeight
        {
            get
            {
                if (_totalWeight < 1)
                    _totalWeight = monsterSpawnGroups.Sum(obj => obj.weight);
                return _totalWeight;
            }
        }
    }

    /// <summary>
    ///     같이 소환되는 몬스터 그룹
    /// </summary>
    [Serializable]
    public struct SpawnGroup
    {
        /// <summary>
        ///     해당 그룹이 선택될 가중치
        /// </summary>
        public int weight;

        /// <summary>
        ///     mosnter reference에서 사용할 에셋 인덱스
        /// </summary>
        public int[] monsterIndex;

        /// <summary>
        ///     monster Stats에서 사용할 스텟 인덱스
        /// </summary>
        public int[] monsterStatIndex;
    }
}