using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using ScriptableObjects.Scripts.Creature;
using Unit.Stages.Creatures.Characters.Unit.Character;
using Unit.Stages.Creatures.FSM;
using Unit.Stages.Creatures.Interfaces;
using UnityEngine;

namespace Unit.Stages.Creatures.Characters {
    public class PlayerCreature : BaseCreature {
        [SerializeField] private StateDataDTO stateData;

        public override Animator Animator => _animator;
        public override BattleSystem Battle => _battleSystem;
        public override HealthSystem Health => _healthSystem;
        public override MovementSystem Movement => _movementSystem;

        public override LinkedList<ModifyStatData> ModifiedStatData => _mods;

        private BattleSystem _battleSystem;
        private HealthSystem _healthSystem;
        private MovementSystem _movementSystem;
        private Animator _animator;
        private LinkedList<ModifyStatData> _mods = new LinkedList<ModifyStatData>();

        private Stat<CharacterStat> _stats;
        private UserInput _input;

        public void Initialize(CharacterStat stat, float groundYPosition, params ActOnInput[] acts) {
            _animator = GetComponent<Animator>();
            _stats = new Stat<CharacterStat>(stat);

            _battleSystem = new BattleSystem(this, _stats);
            _healthSystem = new HealthSystem(this, _stats);
            _movementSystem = new MovementSystem(this, _stats);
            _movementSystem.SetGroundPosition(groundYPosition);

            HFSM = StateBuilder.BuildState(this, stateData);
            _input = new UserInput(this, acts);
        }

        public void Input(NewBlock block, int count) {
            _input.Input(block, count);
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