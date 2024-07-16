using System;
using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Stages;
using Unit.GameScene.Stages.Creautres.Monsters;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers.Modules {
    [Serializable]
    [CreateAssetMenu(fileName = nameof(MonsterSpawnData), menuName = "SO/" + nameof(MonsterSpawnData))]
    public class MonsterSpawnData : ScriptableObject {
        [Header("몬스터 오브젝트")] public Monster[] monstersRef;

        [Header("몬스터 스텟")] public List<MonsterStat> monsterStats;

        [Header("몬스터 스폰 오프셋")] public Vector3 monsterSpawnOffset;

        [Header("몬스터 스폰 타입 오프셋")] public float monsterSpawnTimeOffset;

        [Header("몬스터 스폰에 대한 데이터")] public List<MonsterGroup> monsterGroup;

        public StageMonsterSpawnData GetStageData() {
            List<StageMonsterGroup> monGroup = new List<StageMonsterGroup>();
            foreach (var monster in monsterGroup) {
                monGroup.Add(monster.GetStageMonsterGroup());
            }
            var ret = new StageMonsterSpawnData(monstersRef, monsterStats.ToArray(), monsterSpawnOffset, monsterSpawnTimeOffset, monGroup.ToArray());
            return ret;
        }
    }

    public class StageMonsterSpawnData {
        public Monster[] monstersRef;
        public MonsterStat[] monsterStats;
        public Vector3 monsterSpawnOffset;
        public float monsterSpawnTimeOffset;
        public StageMonsterGroup[] monsterGroup;

        public StageMonsterSpawnData(Monster[] monstersRef, MonsterStat[] monsterStats, Vector3 monsterSpawnOffset, float monsterSpawnTimeOffset, StageMonsterGroup[] monsterGroup) {
            this.monstersRef = monstersRef;
            this.monsterStats = monsterStats;
            this.monsterSpawnOffset = monsterSpawnOffset;
            this.monsterSpawnTimeOffset = monsterSpawnTimeOffset;
            this.monsterGroup = monsterGroup;
        }
    }

    /// <summary>
    ///     Decider에 의해 소환될 몬스터 그룹
    /// </summary>
    [Serializable]
    public class MonsterGroup {
        public SpawnDecider spawnDecider;
        public List<SpawnGroup> monsterSpawnGroups;
        private int _totalWeight;

        public int TotalWeight {
            get {
                if (_totalWeight < 1)
                    _totalWeight = monsterSpawnGroups.Sum(obj => obj.weight);
                return _totalWeight;
            }
        }

        public StageMonsterGroup GetStageMonsterGroup() {
            var ret = new StageMonsterGroup(spawnDecider.GetMonsterSpawnDecider(), monsterSpawnGroups.ToArray(), TotalWeight);
            return ret;
        }
    }

    public class StageMonsterGroup {
        public IMonsterSpawnDecider spawnDecider;
        public SpawnGroup[] monsterSpawnGroups;
        public int TotalWeight;

        public StageMonsterGroup(IMonsterSpawnDecider spawnDecider, SpawnGroup[] monsterSpawnGroups, int totalWeight) {
            this.spawnDecider = spawnDecider;
            this.monsterSpawnGroups = monsterSpawnGroups;
            TotalWeight = totalWeight;
        }
    }

    /// <summary>
    ///     같이 소환되는 몬스터 그룹
    /// </summary>
    [Serializable]
    public struct SpawnGroup {
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