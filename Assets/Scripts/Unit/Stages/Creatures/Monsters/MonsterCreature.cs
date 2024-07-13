using ScriptableObjects.Scripts.Creature;
using System.Collections;
using System.Collections.Generic;
using Unit.Stages.Creatures.FSM;
using Unit.Stages.Creatures.Interfaces;
using UnityEngine;

namespace Unit.Stages.Creatures.Monsters {
    public class MonsterCreature : BaseCreature {
        [SerializeField] private StateDataDTO stateData;

        public override Animator Animator => _animator;
        public override BattleSystem Battle => _battleSystem;
        public override IDamageable Health => _healthSystem as IDamageable;
        public override IRunnable Movement => _movementSystem as IRunnable;

        public override LinkedList<ModifyStatData> ModifiedStatData => _mods;

        private BattleSystem _battleSystem;
        private HealthSystem _healthSystem;
        private MovementSystem _movementSystem;
        private Animator _animator;
        private LinkedList<ModifyStatData> _mods = new LinkedList<ModifyStatData>();

        private Stat<MonsterStat> _stats;

        public void Initialize(MonsterStat stat) {
            _animator = gameObject.GetComponent<Animator>();
            _stats = new Stat<MonsterStat>(stat);

            _battleSystem = new BattleSystem(this, _stats);
            _healthSystem = new HealthSystem(this, _stats);
            _movementSystem = new MovementSystem(this, _stats);

            HFSM = StateBuilder.BuildState(this, stateData);
        }
        private void Update() {
            HFSM?.Update(this);
            Movement?.Update();
        }

        private void FixedUpdate() {
            HFSM?.FixedUpdate(this);
            Movement?.FixedUpdate();
        }
        public override void PermanentModifyStat(EStatType statType, int value) {
            _mods.AddLast(new ModifyStatData(statType, value));
            ModifyStat(statType, value);
        }

        protected void ModifyStat(EStatType statType, int value) {
            var cur = _stats.Current;
            switch (statType) {
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

        public override void TempModifyStat(EStatType statType, int value, float duration) {
            StartCoroutine(TempModifyStatCoroutine(statType, value, duration));
        }

        IEnumerator TempModifyStatCoroutine(EStatType statType, int value, float duration) {
            var data = new TempModifyStatData(statType, value, duration);
            var node = _mods.AddLast(data);
            ModifyStat(statType, value);
            while (duration <= 0) {
                duration -= Time.deltaTime;
                data.Duration = duration;
                yield return null;
            }
            _mods.Remove(node);
            ModifyStat(statType, -value);
        }
    }
}