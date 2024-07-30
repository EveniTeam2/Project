using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Monsters
{
    public class Monster : Creature, IMonsterFsmController
    {
        public event Action OnAttack;

        [SerializeField] private MonsterStateMachineDTO stateData;

        private SpriteRenderer _spriteRenderer;
        private CreatureStat<MonsterStat> CreatureStats;

        private MonsterBattleStat _battleStat;
        private MonsterBattleSystem _battleSystem;
        private MonsterHealthSystem _healthSystem;
        private MonsterMovementSystem _movementSystem;

        public override HealthSystem BaseHealthSystem => _healthSystem;

        public override BattleSystem BaseBattleSystem => _battleSystem;

        public override MovementSystem BaseMovementSystem => _movementSystem;

        private void Update()
        {
            FsmSystem?.Update();
            _movementSystem?.Update();
            _battleSystem?.Update();
        }

        private void FixedUpdate()
        {
            FsmSystem?.FixedUpdate();
            _movementSystem?.FixedUpdate();
        }

        public void Initialize(MonsterStat stat, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            var target = transform;

            AnimatorSystem = GetComponent<AnimatorSystem>();
            AnimatorSystem.Initialize(animationParameter);
            AnimatorSystem.OnAttack += OnAttack;

            _spriteRenderer = GetComponent<SpriteRenderer>();

            CreatureStats = new CreatureStat<MonsterStat>(stat);
            _battleStat = new MonsterBattleStat(CreatureStats);
            _battleSystem = new MonsterBattleSystem(target, _battleStat);
            _healthSystem = new MonsterHealthSystem(new MonsterHealthStat(CreatureStats));
            _movementSystem = new MonsterMovementSystem(target, new MonsterMovementStat(CreatureStats), groundYPosition);

            FsmSystem = StateBuilder.BuildMonsterStateMachine(stateData, this, animationParameter, target);

            Mods = new LinkedList<ModifyStatData>();

            _healthSystem.RegisterOnDamageEvent(CheckAndTransitToHit);
            _healthSystem.RegisterOnDeathEvent(Die);
        }

        private void CheckAndTransitToHit()
        {
            FsmSystem.TryChangeState(StateType.Hit);
            _movementSystem.SetImpact(new Vector2(.3f, .2f), 1);
        }

        private void Die()
        {
            FsmSystem.TryChangeState(StateType.Die);
        }

        public override void PermanentModifyStat(EStatType statType, int value)
        {
            Mods.AddLast(new ModifyStatData(statType, value));
            ModifyStat(statType, value);
        }

        public override void TempModifyStat(EStatType statType, int value, float duration)
        {
            StartCoroutine(TempModifyStatCoroutine(statType, value, duration));
        }

        public override void ClearModifiedStat()
        {
            Mods.Clear();
            CreatureStats.SetCurrent(CreatureStats.Origin);
            StopAllCoroutines();
        }

        protected override void ModifyStat(EStatType statType, int value)
        {
            var cur = CreatureStats.Current;

            switch (statType)
            {
                case EStatType.Health:
                    cur.Health += value;
                    break;
                case EStatType.Attack:
                    cur.Attack += value;
                    break;
                case EStatType.Speed:
                    cur.Speed += value;
                    break;
                case EStatType.CoolTime:
                    cur.CoolTime += value;
                    break;
                default:
                    return;
            }

            CreatureStats.SetCurrent(cur);
        }

        public void SpawnInit(MonsterStat monsterStat)
        {
            CreatureStats = new CreatureStat<MonsterStat>(monsterStat);
            _battleSystem.SpawnInit(new MonsterBattleStat(CreatureStats));
            _healthSystem.SpawnInit(new MonsterHealthStat(CreatureStats));
            _movementSystem.SpawnInit(new MonsterMovementStat(CreatureStats));
            FsmSystem.TryChangeState(StateType.Run);
            ClearModifiedStat();
            _spriteRenderer.color = Color.white;
        }

        internal void RegisterEventDeath(Action<Monster> release)
        {
            FsmSystem.RegisterOnDeathState(() => release.Invoke(this));
        }

        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float range, out RaycastHit2D[] enemies)
        {
            return _battleSystem.CheckEnemyInRange(targetLayer, direction, range, out enemies);
        }

        public void ToggleMovement(bool setRunning)
        {
            _movementSystem.SetRun(setRunning);
        }

        public void SetBool(int parameter, bool value, Action action)
        {
            AnimatorSystem.SetBool(parameter, value, null);
        }

        public void SetTrigger(int parameter, Action action)
        {
            AnimatorSystem.SetTrigger(parameter, null);
        }

        public void SetInteger(int parameter, int value, Action action)
        {
            AnimatorSystem.SetInteger(parameter, value, null);
        }

        public bool IsReadyForAttack()
        {
            return _battleSystem.IsReadyForAttack;
        }

        public IBattleStat GetBattleStat()
        {
            return _battleSystem.GetBattleStat();
        }

        public void Attack(RaycastHit2D target)
        {
            _battleSystem.Attack(target);
        }
    }
}