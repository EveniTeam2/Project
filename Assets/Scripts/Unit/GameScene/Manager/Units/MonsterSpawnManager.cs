using System.Collections.Generic;
using Core.Utils;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Manager.Units.StageManagers.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Monsters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit.GameScene.Manager.Units
{
    public class MonsterSpawnManager
    {
        protected readonly StageMonsterSpawnData _data;
        private readonly Transform playerPosition;
        private readonly float _ground;
        private Dictionary<int, CustomPool<Monster>> _monsterPool;
        private LinkedList<StageMonsterGroup> _spawnGroup;
        private Queue<StageMonsterGroup> _waitGroup;
        private Dictionary<AnimationParameterEnums, int> _animationParameter;
        private bool _onSpawn;
        readonly float _offsetTime = 0;
        float _spawnTime = 0;

        public LinkedList<Monster> Monsters
        {
            get
            {
                LinkedList<Monster> spawnedMonster = new LinkedList<Monster>();
                spawnedMonster.Clear();
                foreach (var (key, pool) in _monsterPool)
                {
                    foreach (var creature in pool.UsedList)
                        spawnedMonster.AddLast(creature);
                }
                return spawnedMonster;

            }
        }

        public MonsterSpawnManager(Transform playerPosition, MonsterSpawnData data, float ground, StageScore score,
            Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            _data = data.GetStageData(score);
            this.playerPosition = playerPosition;
            _ground = ground;
            _spawnGroup = new LinkedList<StageMonsterGroup>();
            _waitGroup = new Queue<StageMonsterGroup>();
            _animationParameter = animationParameter;
            CreateMonsterPool(_data, ground);
            this._offsetTime = data.monsterSpawnTimeOffset;
        }

        private void CreateMonsterPool(StageMonsterSpawnData data, float ground)
        {
            _monsterPool = new Dictionary<int, CustomPool<Monster>>();
            for (int i = 0; i < data.monstersRef.Length; ++i)
            {
                int index = i;
                _monsterPool.Add(index, new CustomPool<Monster>(data.monstersRef[index], null,
                    (monCreate, pool) =>
                    {
                        var returnPool = pool;
                        monCreate.Initialize(_data.monsterStats[index], ground, _animationParameter);
                        monCreate.gameObject.SetActive(false);
                        monCreate.RegistEventDeath(returnPool.Release);
                    },
                    monGet => { monGet.ClearModifiedStat(); },
                    monRelease => { monRelease.gameObject.SetActive(false); },
                    monDestroy => { },
                    5, true));
            }
        }

        public void Start()
        {
            _onSpawn = true;
            InitializeDecider();
            _waitGroup.Clear();
            foreach (var group in _data.monsterGroup)
            {
                _waitGroup.Enqueue(group);
            }
        }

        public void Update()
        {
            _spawnTime += Time.deltaTime;
            if (_onSpawn && _spawnTime > _offsetTime)
            {
                CheckConditionAndSpawn();
                _spawnTime = 0;
            }
        }

        private void InitializeDecider()
        {
            for (int i = 0; i < _data.monsterGroup.Length; ++i)
            {
                _data.monsterGroup[i].spawnDecider.Initialize();
            }
        }

        private void CheckConditionAndSpawn()
        {
            CheckWaitList();
            var group = _spawnGroup.First;
            while (group != null)
                if (group.Value.spawnDecider.CanExecute())
                {
                    RandomSpawnMonster(group.Value);
                    group = group.Next;
                }
                else
                {
                    var deleteGroup = group;
                    group = group.Next;
                    _spawnGroup.Remove(deleteGroup);
                }
        }

        private void RandomSpawnMonster(StageMonsterGroup value)
        {
            int weight = 0;
            int rand = Random.Range(0, value.TotalWeight);
            foreach (var group in value.monsterSpawnGroups)
            {
                weight += group.weight;
                if (rand < weight)
                {
                    SpawnMonster(group);
                    break;
                }
            }
        }

        private void CheckWaitList()
        {
            if (_waitGroup.Count > 0)
                if (_waitGroup.Peek().spawnDecider.CanExecute())
                {
                    var group = _waitGroup.Dequeue();
                    _spawnGroup.AddLast(group);
                }
        }

        private void SpawnMonster(SpawnGroup group)
        {
            for (var i = 0; i < group.monsterIndex.Length; ++i)
                if (_monsterPool.TryGetValue(group.monsterIndex[i], out var pool))
                {
                    Debug.Assert(_data.monsterStats.Length > group.monsterStatIndex[i],
                        $"{_data.monsterStats.Length}|{group.monsterStatIndex[i]} 문제 발생!!");
                    var monster = pool.Get();
                    monster.transform.position = _data.monsterSpawnOffset + playerPosition.position +
                                                 new Vector3(Random.Range(-1f, 1f), 0f);
                    monster.gameObject.SetActive(true);
                    monster.SpawnInit(_data.monsterStats[group.monsterStatIndex[i]]);
                }
        }

        public void Clear()
        {
            foreach (var mon in _monsterPool)
            {
                mon.Value.DestroyPool();
            }
        }
    }
}