using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Monsters
{
    public class Monster : Creature
    {
        [SerializeField] private MonsterStateMachineDTO stateData;
        private MonsterServiceProvider _monsterServiceProvider;
        private SpriteRenderer _spriteRenderer;
        protected Stat<MonsterStat> _stats;
        private MonsterBattleStat _battleStat;

        private void Update()
        {
            _fsm?.Update();
            _movementSystem?.Update();
            _battleSystem?.Update();
        }

        private void FixedUpdate()
        {
            _fsm?.FixedUpdate();
            _movementSystem?.FixedUpdate();
        }

        public void Initialize(MonsterStat stat, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            _animatorEventReceiver = GetComponent<AnimatorEventReceiver>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _stats = new Stat<MonsterStat>(stat);
            _battleStat = new MonsterBattleStat(_stats);
            _battleSystem = new MonsterBattleSystem(transform, _battleStat);
            _healthSystem = new MonsterHealthSystem(new MonsterHealthStat(_stats));
            _movementSystem = new MonsterMovementSystem(transform, new MonsterMovementStat(_stats), groundYPosition);

            _fsm = StateBuilder.BuildStateMachine(stateData, transform, _battleSystem, _healthSystem, _movementSystem, _animatorEventReceiver, animationParameter);

            _monsterServiceProvider = new MonsterServiceProvider(_battleSystem, _healthSystem, _movementSystem, _animatorEventReceiver, _fsm, animationParameter, null);
            _mods = new LinkedList<ModifyStatData>();

            _healthSystem.RegistOnDamageEvent(CheckAndTransitToHit);
            _healthSystem.RegistOnDeathEvent(Die);
        }

        private void CheckAndTransitToHit()
        {
            _fsm.TryChangeState(StateType.Hit);
        }

        private void Die()
        {
            _fsm.TryChangeState(StateType.Die);
        }

        public override void PermanentModifyStat(EStatType statType, int value)
        {
            _mods.AddLast(new ModifyStatData(statType, value));
            ModifyStat(statType, value);
        }

        public override void TempModifyStat(EStatType statType, int value, float duration)
        {
            StartCoroutine(TempModifyStatCoroutine(statType, value, duration));
        }

        public override void ClearModifiedStat()
        {
            _mods.Clear();
            _stats.SetCurrent(_stats.Origin);
            StopAllCoroutines();
        }

        protected override void ModifyStat(EStatType statType, int value)
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

        public void SpawnInit(MonsterStat monsterStat)
        {
            _stats = new Stat<MonsterStat>(monsterStat);
            _battleSystem.SpawnInit(new MonsterBattleStat(_stats));
            _healthSystem.SpawnInit(new MonsterHealthStat(_stats));
            _movementSystem.SpawnInit(new MonsterMovementStat(_stats));
            _fsm.TryChangeState(StateType.Run);
            ClearModifiedStat();
            _spriteRenderer.color = Color.white;
        }

        internal void RegistEventDeath(Action<Monster> release)
        {
            _fsm.RegistOnDeathState(() => release.Invoke(this));
        }
    }
}