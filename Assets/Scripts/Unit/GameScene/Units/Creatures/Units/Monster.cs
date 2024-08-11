using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Data.MonsterDatas;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.Battles;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Creatures.Units
{
    public interface IEventSubscriber
    {
        public void OnEvent();
    }

    public interface IEventPublisher
    {
        public void RegisterEvent(IEventSubscriber eventSubscriber);
        public void UnregisterEvent(IEventSubscriber eventSubscriber);
    }

    public class ManualUpdater : IEventPublisher
    {
        private event Action UpdateEvent;
        public void Update()
        {
            UpdateEvent?.Invoke();
        }
        public void RegisterEvent(IEventSubscriber eventSubscriber)
        {
            UpdateEvent += eventSubscriber.OnEvent;
        }
        public void UnregisterEvent(IEventSubscriber eventSubscriber)
        {
            UpdateEvent -= eventSubscriber.OnEvent;
        }
    }
    public class Monster : Creature, IMonsterFsmController, ITakePlayerDamage
    {
        [SerializeField] private MonsterStateMachineDTO stateData;
        [SerializeField] private RectTransform monsterHpPanelUI;
        [SerializeField] private RectTransform monsterHpHandler;

        protected override AnimatorSystem AnimatorSystem { get; set; }
        protected override Collider2D CreatureCollider { get; set; }
        protected override RectTransform CreatureHpHandler { get; set; }
        protected override RectMask2D CreatureHpHandlerMask { get; set; }

        private MonsterBattleSystem _monsterBattleSystem;
        private MonsterMovementSystem _monsterMovementSystem;
        private MonsterStatSystem _monsterStatsSystem;

        private SpriteRenderer _spriteRenderer;
        private ManualUpdater ManualUpdate;

        public int GetDamage() => _monsterStatsSystem.Damage;
        public bool IsReadyForAttack() => _monsterBattleSystem.IsReadyForAttack;
        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float range, out RaycastHit2D[] enemies) => _monsterBattleSystem.CheckEnemyInRange(targetLayer, direction, range, out enemies);

        public void Initialize(MonsterStat stat, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameters)
        {
            var monsterTransform = transform;
            AnimationParameters = animationParameters;

            CreatureHpHandler = monsterHpHandler;
            CreatureHpHandlerMask = CreatureHpHandler.GetComponent<RectMask2D>();

            AnimatorSystem = GetComponent<AnimatorSystem>();
            CreatureCollider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _monsterStatsSystem = new MonsterStatSystem(stat);
            _monsterBattleSystem = new MonsterBattleSystem(_monsterStatsSystem, monsterTransform);
            _monsterMovementSystem = new MonsterMovementSystem(_monsterStatsSystem, monsterTransform, groundYPosition);

            AnimatorSystem.Initialize(AnimationParameters);

            FsmSystem = StateBuilder.BuildMonsterStateMachine(stateData, this, AnimationParameters, monsterTransform, _monsterStatsSystem);

            RegisterEventHandler();
            SetActiveHealthBarUI(true);
            _monsterStatsSystem.InitializeStat(this);
        }

        public void ResetMonster()
        {
            AnimatorSystem.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], false, null);
            FsmSystem.TryChangeState(StateType.Run);
            _spriteRenderer.color = Color.white;

            SetActiveCollider(true);
            RegisterEventHandler();
            SetActiveHealthBarUI(true);

            _monsterStatsSystem.InitializeStat(this);
        }

        private void Update()
        {
            FsmSystem?.Update();
            _monsterMovementSystem?.Update();
            _monsterBattleSystem?.Update();
            ManualUpdate.Update();
        }

        private void FixedUpdate()
        {
            FsmSystem?.FixedUpdate();
            _monsterMovementSystem?.FixedUpdate();
        }

        protected override void RegisterEventHandler()
        {
            _monsterStatsSystem.RegisterHandleOnUpdateHpPanelUI(UpdateHpBar);
            _monsterStatsSystem.RegisterHandleOnDeath(HandleOnDeath);
            _monsterStatsSystem.RegisterHandleOnHit(HandleOnHit);
        }

        public void RegisterOnAttackEventHandler(Action onAttack)
        {
            AnimatorSystem.OnAttack += onAttack;
        }

        public void UnregisterOnAttackEventHandler(Action onAttack)
        {
            AnimatorSystem.OnAttack -= onAttack;
        }

        protected override void HandleOnHit()
        {
            FsmSystem.TryChangeState(StateType.Hit);
        }

        protected override void HandleOnDeath()
        {
            AnimatorSystem.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], true, null);
            SetActiveHealthBarUI(false);
            SetActiveCollider(false);

            FsmSystem.TryChangeState(StateType.Die);
        }

        internal void RegisterEventDeath(Action<Monster> release)
        {
            FsmSystem.RegisterHandleOnDeathState(() => release.Invoke(this));
        }

        public void ToggleMovement(bool setRunning)
        {
            _monsterMovementSystem.SetRun(setRunning);
        }

        public void AttackEnemy(RaycastHit2D target)
        {
            _monsterBattleSystem.MonsterAttackEnemy(GetDamage(), target);
        }

        public Func<int, int> TakeDamage()
        {
            return damage =>
            {
                _monsterMovementSystem.SetImpact();
                _monsterStatsSystem.RegisterHandleOnUpdatePermanentStat(StatType.CurrentHp, damage * -1);

                return _monsterStatsSystem.ReturnExp();
            };
        }

        private void SetActiveHealthBarUI(bool active)
        {
            monsterHpPanelUI.gameObject.SetActive(active);
        }

        bool IMonsterFsmController.CheckPlayer(Vector2 startPos, Vector2 endPos, out RaycastHit2D[] target)
        {
            Vector2 direction = endPos - startPos;
            float distance = direction.magnitude;
            direction.Normalize();
            return _monsterBattleSystem.CheckEnemyInRange(1 << LayerMask.NameToLayer("Player"), direction, distance, out target);
        }

        bool IMonsterFsmController.CheckPlayer(Vector2 direction, float distance, out RaycastHit2D[] target)
        {
            return _monsterBattleSystem.CheckEnemyInRange(1 << LayerMask.NameToLayer("Player"), direction, distance, out target);
        }
    }
}