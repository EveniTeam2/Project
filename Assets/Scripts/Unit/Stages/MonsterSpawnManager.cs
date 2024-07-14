using Core.Utils;
using System;
using System.Collections.Generic;
using Unit.Stages.Creatures.Monsters;
using Unit.Stages.Interfaces;

namespace Unit.Stages {
    public partial class MonsterSpawnManager {
        private Dictionary<string, CustomPool<MonsterCreature>> _monsterPool;
        public List<MonsterCreature> Monsters {
            get {
                List<MonsterCreature> monsters = new List<MonsterCreature>();
                foreach (var (key, pool) in _monsterPool) {
                    monsters.AddRange(pool.UsedList);
                }
                return monsters;
            }
        }
        /// <summary>
        /// 해당 구간에서 랜덤 소환 그룹 모음
        /// </summary>
        [Serializable]
        public struct RandomMonsterGroup {
            public float minDistance;
            public float maxDistance;
            public List<MonsterSpawnGroup> monsterSpawnGroups;
        }
        /// <summary>
        /// 해당 거리에서 확정 소환 그룹 모음
        /// </summary>
        [Serializable]
        public struct StaticMonsterGroup {
            public float minDistance;
            public List<MonsterSpawnGroup> monsterSpawnGroups;
        }
        /// <summary>
        /// 같이 소환되는 몬스터 그룹
        /// </summary>
        [Serializable]
        public struct MonsterSpawnGroup {
            /// <summary>
            /// mosnter reference에서 사용할 에셋 인덱스
            /// </summary>
            public int[] monsterIndex;
            /// <summary>
            /// monster Stats에서 사용할 스텟 인덱스
            /// </summary>
            public int[] monsterStatIndex;
        }
        protected MonsterSpawnData _data;
        protected StageManager _parent;
        private float _ground;

        public MonsterSpawnManager(StageManager parent, MonsterSpawnData data, float ground) {
            _data = data;
            _parent = parent;
            _ground = ground;
            _monsterPool = new Dictionary<string, CustomPool<MonsterCreature>>();

            for (int i = 0; i < data.monstersRef.Length; ++i) {
                int index = i;
                _monsterPool.Add(data.monstersRef[index].name, new CustomPool<MonsterCreature>(data.monstersRef[index], null,
                    (monCreate, pool) => {
                        monCreate.Initialize(_data.monsterStats[index], ground);
                        monCreate.gameObject.SetActive(false);
                    },
                    monGet => {
                        monGet.ClearStat();
                    },
                    monRelease => { },
                    monDestroy => { },
                    5, true));
                //Core.Utils.AddressableLoader.DeployAsset(settings.monstersRef[i], settings.monsterSpawnOffset, Quaternion.identity, null, (obj) => {
                //    if (obj.TryGetComponent(out MonsterCreature mon))
                //    {
                //        _monsters.Add(mon);
                //        mon.Initialize(settings.monsterStats[index], groundYPosition);
                //    }
                //});
            }
        }
    }
}