using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Data.MonsterDatas;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.Battles;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Creatures.Units
{
    public class Monster : Creature, IMonsterSkillController
    {
        [SerializeField] private MonsterType monsterType;
        [SerializeField] private RectTransform monsterHpPanelUI;
        [SerializeField] private RectTransform monsterHpHandler;

        protected override AnimationEventReceiver AnimationEventReceiver { get; set; }
        protected override Collider2D CreatureCollider { get; set; }
        protected override RectTransform CreatureHpHandler { get; set; }
        protected override RectMask2D CreatureHpHandlerMask { get; set; }

        private MonsterData _monsterData;
        private MonsterStatSystem _monsterStatSystem;
        private MonsterSkillSystem _monsterSkillSystem;
        private MonsterBattleSystem _monsterBattleSystem;
        private MonsterMovementSystem _monsterMovementSystem;

        private SpriteRenderer _spriteRenderer;

        public int GetDamage() => _monsterStatSystem.Damage;
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

        public void Initialize(MonsterData monsterData, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameters)
        {
            var monsterTransform = transform;
            
            CreatureCollider = GetComponent<Collider2D>();
            AnimationEventReceiver = GetComponent<AnimationEventReceiver>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            AnimationParameters = animationParameters;
            

            _monsterData = monsterData;
            // monsterType = _monsterData.MonsterDataSo.type;
            //
            // _monsterStatSystem = _monsterData.StatSystem;
            // _monsterSkillSystem = _monsterData.SkillSystem;
            _monsterBattleSystem = new MonsterBattleSystem(_monsterStatSystem, monsterTransform);
            _monsterMovementSystem = new MonsterMovementSystem(_monsterStatSystem, monsterTransform, groundYPosition);

            AnimationEventReceiver.Initialize(AnimationParameters);

            RegisterEventHandler();
            SetActiveHealthBarUI(true);
            
            CreatureHpHandlerMask = CreatureHpHandler.GetComponent<RectMask2D>();
            
            _monsterStatSystem.InitializeStat(this);
        }
        
        private void RegisterEventHandler()
        {
            CreatureHpHandler = monsterHpHandler;
            
            _monsterStatSystem.RegisterHandleOnUpdateHpPanelUI(UpdateHpBar);
            _monsterStatSystem.RegisterHandleOnDeath(HandleOnDeath);
            _monsterStatSystem.RegisterHandleOnHit(HandleOnHit);
        }

        public void ResetMonster()
        {
            AnimationEventReceiver.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], false, null);
            StateMachine.ChangeState(StateType.Run);
            _spriteRenderer.color = Color.white;

            SetActiveCollider(true);
            RegisterEventHandler();
            SetActiveHealthBarUI(true);

            _monsterStatSystem.InitializeStat(this);
        }

        private void Update()
        {
            StateMachine?.Update();
            _monsterMovementSystem?.Update();
            _monsterBattleSystem?.Update();
        }

        private void FixedUpdate()
        {
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
            StateMachine.ChangeState(StateType.Hit);
        }

        protected override void HandleOnDeath()
        {
            AnimationEventReceiver.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], true, null);
            SetActiveHealthBarUI(false);
            SetActiveCollider(false);

            StateMachine.ChangeState(StateType.Die);
        }

        protected void AttackEnemy(int value, float range)
        {
            _monsterBattleSystem.AttackEnemy(value, range);
        }

        // internal void RegisterEventDeath(Action<Monster> release)
        // {
        //     StateMachine.RegisterHandleOnDeathState(() => release.Invoke(this));
        // }

        public void ToggleMovement(bool setRunning)
        {
            _monsterMovementSystem.SetRun(setRunning);
        }
        
        public Func<int, int> TakeDamage()
        {
            return damage =>
            {
                _monsterMovementSystem.SetImpact();
                _monsterStatSystem.RegisterHandleOnUpdatePermanentStat(StatType.CurrentHp, damage * -1);

                return _monsterStatSystem.ReturnExp();
            };
        }

        private void SetActiveHealthBarUI(bool active)
        {
            monsterHpPanelUI.gameObject.SetActive(active);
        }
    }
}