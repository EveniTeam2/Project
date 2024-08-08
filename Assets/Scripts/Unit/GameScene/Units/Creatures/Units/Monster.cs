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
using UnityEngine;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Creatures.Units
{
    public class Monster : Creature, IMonsterSkillController
    {
        [SerializeField] private RectTransform monsterHpPanelUI;
        [SerializeField] private RectTransform monsterHpHandler;

        protected override AnimationEventReceiver AnimationEventReceiver { get; set; }
        protected override Collider2D CreatureCollider { get; set; }
        protected override RectTransform CreatureHpHandler { get; set; }
        protected override RectMask2D CreatureHpHandlerMask { get; set; }

        private MonsterBattleSystem _monsterBattleSystem;
        private MonsterMovementSystem _monsterMovementSystem;
        private MonsterStatSystem _monsterStatsSystem;

        private SpriteRenderer _spriteRenderer;

        public int GetDamage() => _monsterStatsSystem.Damage;
        public bool IsReadyForAttack() => _monsterBattleSystem.IsReadyForAttack;

        public void Attack(int value, float range)
        {
            throw new NotImplementedException();
        }

        public void Heal(int value)
        {
            throw new NotImplementedException();
        }

        public void Buff(StatType statType, int value, float duration)
        {
            throw new NotImplementedException();
        }

        public bool CheckEnemyInRange(float range, out RaycastHit2D[] enemies) => _monsterBattleSystem.CheckEnemyInRange(range, out enemies);

        public void Initialize(MonsterStat stat, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameters)
        {
            var monsterTransform = transform;
            AnimationParameters = animationParameters;
            
            CreatureHpHandlerMask = CreatureHpHandler.GetComponent<RectMask2D>();

            AnimationEventReceiver = GetComponent<AnimationEventReceiver>();
            CreatureCollider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _monsterStatsSystem = new MonsterStatSystem(stat);
            _monsterBattleSystem = new MonsterBattleSystem(_monsterStatsSystem, monsterTransform);
            _monsterMovementSystem = new MonsterMovementSystem(_monsterStatsSystem, monsterTransform, groundYPosition);

            AnimationEventReceiver.Initialize(AnimationParameters);

            RegisterEventHandler();
            SetActiveHealthBarUI(true);
            
            _monsterStatsSystem.InitializeStat(this);
        }
        
        private void RegisterEventHandler()
        {
            CreatureHpHandler = monsterHpHandler;
            
            _monsterStatsSystem.RegisterHandleOnUpdateHpPanelUI(UpdateHpBar);
            _monsterStatsSystem.RegisterHandleOnDeath(HandleOnDeath);
            _monsterStatsSystem.RegisterHandleOnHit(HandleOnHit);
        }

        public void ResetMonster()
        {
            AnimationEventReceiver.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], false, null);
            StateMachine.TryChangeState(StateType.Run);
            _spriteRenderer.color = Color.white;

            SetActiveCollider(true);
            RegisterEventHandler();
            SetActiveHealthBarUI(true);

            _monsterStatsSystem.InitializeStat(this);
        }

        private void Update()
        {
            StateMachine?.Update();
            _monsterMovementSystem?.Update();
            _monsterBattleSystem?.Update();
        }

        private void FixedUpdate()
        {
            StateMachine?.FixedUpdate();
            _monsterMovementSystem?.FixedUpdate();
        }
        
        public void RegisterOnAttackEventHandler(Action onAttack)
        {
            AnimationEventReceiver.OnActivateSkillEffect += onAttack;
        }

        public void UnregisterOnAttackEventHandler(Action onAttack)
        {
            AnimationEventReceiver.OnActivateSkillEffect -= onAttack;
        }

        protected override void HandleOnHit()
        {
            StateMachine.TryChangeState(StateType.Hit);
        }

        protected override void HandleOnDeath()
        {
            AnimationEventReceiver.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], true, null);
            SetActiveHealthBarUI(false);
            SetActiveCollider(false);

            StateMachine.TryChangeState(StateType.Die);
        }

        protected void AttackEnemy(int value, float range)
        {
            _monsterBattleSystem.AttackEnemy(value, range);
        }

        internal void RegisterEventDeath(Action<Monster> release)
        {
            StateMachine.RegisterHandleOnDeathState(() => release.Invoke(this));
        }

        public void ToggleMovement(bool setRunning)
        {
            _monsterMovementSystem.SetRun(setRunning);
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
    }
}