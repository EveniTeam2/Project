using System.Collections.Generic;
using Core.Utils;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Manager.Units.StageManagers.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit.GameScene.Manager.Units {
    public class MonsterSpawnManager {
        protected readonly StageManager _stageManager;
        protected readonly StageMonsterSpawnData _data;
        private readonly float _ground;
        private Dictionary<int, CustomPool<Monster>> _monsterPool;
        private LinkedList<Monster> _spawnedMonsters;
        private LinkedList<StageMonsterGroup> _spawnGroup;
        private Queue<StageMonsterGroup> _waitGroup;
        private bool _onSpawn;


        public LinkedList<Monster> Monsters {
            get {
                _spawnedMonsters.Clear();
                foreach (var (key, pool) in _monsterPool) {
                    foreach (var creature in pool.UsedList)
                        _spawnedMonsters.AddLast(creature);
                }
                return _spawnedMonsters;

            }
        }

        public MonsterSpawnManager(StageManager stageManager, MonsterSpawnData data, float ground) {
            _data = data.GetStageData();
            _stageManager = stageManager;
            _ground = ground;
            _spawnGroup = new LinkedList<StageMonsterGroup>();
            _waitGroup = new Queue<StageMonsterGroup>();
            CreateMonsterPool(stageManager, _data, ground);
        }

        private void CreateMonsterPool(StageManager stageManager, StageMonsterSpawnData data, float ground) {
            _monsterPool = new Dictionary<int, CustomPool<Monster>>();
            for (int i = 0; i < data.monstersRef.Length; ++i) {
                int index = i;
                _monsterPool.Add(index, new CustomPool<Monster>(data.monstersRef[index], null,
                    (monCreate, pool) => {
                        monCreate.Initialize(stageManager, _data.monsterStats[index], ground);
                        monCreate.gameObject.SetActive(false);
                    },
                    monGet => { monGet.ClearStat(); },
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

        public StageManager StageManager => _stageManager;

        public void Start() {
            _onSpawn = true;
            InitializeDecider();
            _waitGroup.Clear();
            foreach (var group in _data.monsterGroup) {
                _waitGroup.Enqueue(group);
            }
        }

        public void Update() {
            if (_onSpawn)
                CheckConditionAndSpawn();
        }

        private void InitializeDecider() {
            for (int i = 0; i < _data.monsterGroup.Length; ++i) {
                _data.monsterGroup[i].spawnDecider.Initialize();
            }
        }

        private void CheckConditionAndSpawn() {
            CheckWaitList();
            var group = _spawnGroup.First;
            while (group != null)
                if (group.Value.spawnDecider.Execute(this, group.Value)) {
                    group = group.Next;
                }
                else {
                    var deleteGroup = group;
                    group = group.Next;
                    _spawnGroup.Remove(deleteGroup);
                }
        }

        private void CheckWaitList() {
            if (_waitGroup.Count > 0)
                if (_waitGroup.Peek().spawnDecider.CanExecute(this)) {
                    var group = _waitGroup.Dequeue();
                    _spawnGroup.AddLast(group);
                }
        }

        public void SpawnMonster(SpawnGroup group) {
            for (var i = 0; i < group.monsterIndex.Length; ++i)
                if (_monsterPool.TryGetValue(group.monsterIndex[i], out var pool)) {
                    Debug.Assert(_data.monsterStats.Length > group.monsterStatIndex[i],
                        $"{_data.monsterStats.Length}|{group.monsterStatIndex[i]} 문제 발생!!");
                    var monster = pool.Get();
                    monster.Initialize(_stageManager, _data.monsterStats[group.monsterStatIndex[i]], _ground);
                    monster.transform.position = _data.monsterSpawnOffset + StageManager.Character.transform.position +
                                                 new Vector3(Random.Range(-1f, 1f), 0f);
                    monster.gameObject.SetActive(true);
                    monster.HFSM.TryChangeState(StateType.Run);
                }
        }
    }
}