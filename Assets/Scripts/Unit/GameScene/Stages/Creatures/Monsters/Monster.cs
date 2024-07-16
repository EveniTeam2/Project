using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Stages.Creatures.FSM;
using Unit.GameScene.Stages.Creatures.FSM.ActOnInput;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Monsters
{
    public class Monster : BaseCreature
    {
        [SerializeField] private StateDataDTO stateData;
        private Animator _animator;

        private BattleSystem _battleSystem;
        private HealthSystem _healthSystem;
        private readonly LinkedList<ModifyStatData> _mods = new();
        private MovementSystem _movementSystem;
        private StageManager _stageManager;
        private Stat<MonsterStat> _stats;

        public override Animator Animator => _animator;
        public override BattleSystem Battle => _battleSystem;
        public override HealthSystem Health => _healthSystem;
        public override MovementSystem Movement => _movementSystem;
        public override LinkedList<ModifyStatData> ModifiedStatData => _mods;
        public override StageManager StageManager => _stageManager;

        private void Update()
        {
            HFSM?.Update(this);
            Movement?.Update();
        }

        private void FixedUpdate()
        {
            HFSM?.FixedUpdate(this);
            Movement?.FixedUpdate();
        }

        public void Initialize(StageManager manager, MonsterStat stat, float groundYPosition)
        {
            _animator = gameObject.GetComponent<Animator>();
            _stats = new Stat<MonsterStat>(stat);
            _stageManager = manager;
            _battleSystem = new BattleSystem(manager, this, _stats);
            _healthSystem = new HealthSystem(this, _stats);
            _movementSystem = new MovementSystem(this, _stats);
            _movementSystem.SetGroundPosition(groundYPosition);

            HFSM = StateBuilder.BuildState(this, stateData);
        }

        public override void PermanentModifyStat(EStatType statType, int value)
        {
            _mods.AddLast(new ModifyStatData(statType, value));
            ModifyStat(statType, value);
        }

        protected void ModifyStat(EStatType statType, int value)
        {
            var cur = _stats.Current;
            switch (statType)
            {
                case EStatType.None:
                    break;
                case EStatType.Health:
                    cur.Health += value;
                    break;
                case EStatType.Attack:
                    cur.Attack += value;
                    break;
                case EStatType.Speed:
                    cur.Speed += value;
                    break;
            }

            _stats.SetCurrent(cur);
        }

        public override void TempModifyStat(EStatType statType, int value, float duration)
        {
            StartCoroutine(TempModifyStatCoroutine(statType, value, duration));
        }

        public override void ClearStat()
        {
            _mods.Clear();
            _stats.SetCurrent(_stats.Origin);
            StopAllCoroutines();
        }

        private IEnumerator TempModifyStatCoroutine(EStatType statType, int value, float duration)
        {
            var data = new TempModifyStatData(statType, value, duration);
            var node = _mods.AddLast(data);
            ModifyStat(statType, value);
            while (duration <= 0)
            {
                duration -= Time.deltaTime;
                data.Duration = duration;
                yield return null;
            }

            _mods.Remove(node);
            ModifyStat(statType, -value);
        }
    }
}