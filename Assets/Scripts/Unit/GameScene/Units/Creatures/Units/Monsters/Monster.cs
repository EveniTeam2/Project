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
        private SpriteRenderer _spriteRenderer;
        protected CreatureStat<MonsterStat> CreatureStats;
        private MonsterBattleStat _battleStat;

        MonsterBattleSystem _monsterBattleSystem;
        MonsterHealthSystem _monsterHealthSystem;
        MonsterMovementSystem _monsterMovementSystem;

        protected override BattleSystem BattleSystem => _monsterBattleSystem;

        protected override HealthSystem HealthSystem => _monsterHealthSystem;

        protected override MovementSystem MovementSystem => _monsterMovementSystem;

        private void Update()
        {
            _fsm?.Update();
            MovementSystem?.Update();
            BattleSystem?.Update();
        }

        private void FixedUpdate()
        {
            _fsm?.FixedUpdate();
            MovementSystem?.FixedUpdate();
        }

        public void Initialize(MonsterStat stat, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            _animatorEventReceiver = GetComponent<AnimatorEventReceiver>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            CreatureStats = new CreatureStat<MonsterStat>(stat);
            _battleStat = new MonsterBattleStat(CreatureStats);
            _monsterBattleSystem = new MonsterBattleSystem(transform, _battleStat);
            _monsterHealthSystem = new MonsterHealthSystem(new MonsterHealthStat(CreatureStats));
            _monsterMovementSystem = new MonsterMovementSystem(transform, new MonsterMovementStat(CreatureStats), groundYPosition);

            _fsm = StateBuilder.BuildStateMachine(stateData, transform, BattleSystem, HealthSystem, MovementSystem, _animatorEventReceiver, animationParameter);
            
            _mods = new LinkedList<ModifyStatData>();

            HealthSystem.RegistOnDamageEvent(CheckAndTransitToHit);
            HealthSystem.RegistOnDeathEvent(Die);
        }

        private void CheckAndTransitToHit()
        {
            _fsm.TryChangeState(StateType.Hit);
            MovementSystem.SetImpact(new Vector2(.3f, .2f), 1);
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
            CreatureStats.SetCurrent(CreatureStats.Origin);
            StopAllCoroutines();
        }

        protected override void ModifyStat(EStatType statType, int value)
        {
            var cur = CreatureStats.Current;
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

            CreatureStats.SetCurrent(cur);
        }

        public void SpawnInit(MonsterStat monsterStat)
        {
            CreatureStats = new CreatureStat<MonsterStat>(monsterStat);
            BattleSystem.SpawnInit(new MonsterBattleStat(CreatureStats));
            HealthSystem.SpawnInit(new MonsterHealthStat(CreatureStats));
            MovementSystem.SpawnInit(new MonsterMovementStat(CreatureStats));
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