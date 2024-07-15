using System.Collections.Generic;
using Core.Utils;
using Unit.GameScene.Stages.Creatures.Monsters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit.GameScene.Stages {

    public partial class MonsterSpawnManager {
        private Dictionary<int, CustomPool<MonsterCreature>> _monsterPool;
        public LinkedList<MonsterCreature> Monsters {
            get {
                _spawnedMonsters.Clear();
                foreach (var (key, pool) in _monsterPool) {
                    foreach (var creature in pool.UsedList)
                        _spawnedMonsters.AddLast(creature);
                }
                return _spawnedMonsters;
            }
        }
        private LinkedList<MonsterCreature> _spawnedMonsters = new LinkedList<MonsterCreature>();
        public StageManager StageManager => _stageManager;
        protected StageManager _stageManager;
        protected MonsterSpawnData _data;
        private float _ground;
        private LinkedList<MonsterGroup> _spawnGroup = new LinkedList<MonsterGroup>();
        private Queue<MonsterGroup> _waitGroup;
        private bool _onSpawn;

        public MonsterSpawnManager(StageManager stageManager, MonsterSpawnData data, float ground) {
            _data = data;
            _stageManager = stageManager;
            _ground = ground;
            _monsterPool = new Dictionary<int, CustomPool<MonsterCreature>>();

            for (int i = 0; i < data.monstersRef.Length; ++i) {
                int index = i;
                _monsterPool.Add(index, new CustomPool<MonsterCreature>(data.monstersRef[index], null,
                    (monCreate, pool) => {
                        monCreate.Initialize(stageManager, _data.monsterStats[index], ground);
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
            _waitGroup = new Queue<MonsterGroup>();
            foreach (var group in data.monsterGroup) {
                _waitGroup.Enqueue(group);
            }
        }

        public void Start() {
            _onSpawn = true;
        }

        public void Update() {
            if (_onSpawn)
                CheckConditionAndSpawn();
        }

        private void CheckConditionAndSpawn() {
            CheckWaitList();
            var group = _spawnGroup.First;
            while (group != null) {
                if (group.Value.spawnDecider.Execute(this, group.Value)) {
                    group = group.Next;
                }
                else {
                    var deleteGroup = group;
                    group = group.Next;
                    _spawnGroup.Remove(deleteGroup);
                }
            }
        }

        private void CheckWaitList() {
            if (_waitGroup.Count > 0) {
                if (_waitGroup.Peek().spawnDecider.CanExecute(this)) {
                    var group = _waitGroup.Dequeue();
                    _spawnGroup.AddLast(group);
                }
            }
        }

        public void SpawnMonster(SpawnGroup group) {
            for (int i = 0; i < group.monsterIndex.Length; ++i) {
                if (_monsterPool.TryGetValue(group.monsterIndex[i], out var pool)) {
                    Debug.Assert(_data.monsterStats.Length > group.monsterStatIndex[i], $"{_data.monsterStats.Length}|{group.monsterStatIndex[i]} 문제 발생!!");
                    var monster = pool.Get();
                    monster.Initialize(_stageManager, _data.monsterStats[group.monsterStatIndex[i]], _ground);
                    monster.transform.position = _data.monsterSpawnOffset + StageManager.Character.transform.position + new Vector3(Random.Range(-1f, 1f),0f);
                    monster.gameObject.SetActive(true);
                }
            }
        }
    }
}