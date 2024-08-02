using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Units.Characters;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Datas;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Stats;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Systems;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Creatures.Units.Monsters
{
    public class Monster : Creature, IMonsterFsmController, ITakePlayerDamage
    {
        [SerializeField] private MonsterStateMachineDTO stateData;
        [SerializeField] private RectTransform monsterHpPanel;
        [SerializeField] private RectTransform monsterHpUI;
        
        private MonsterData _monsterData;
        private SpriteRenderer _spriteRenderer;
        
        protected override Collider2D CreatureCollider { get; set; }
        protected override RectTransform CreatureHpPanelUI { get; set; }

        private MonsterBattleSystem _monsterBattleSystem;
        private MonsterHealthSystem _monsterHealthSystem;
        private MonsterMovementSystem _monsterMovementSystem;
        private MonsterStatSystem _monsterStatsSystem;
        
        public int GetDamage() => _monsterStatsSystem.Damage;
        public bool IsReadyForAttack() => _monsterBattleSystem.IsReadyForAttack;
        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float range, out RaycastHit2D[] enemies) => _monsterBattleSystem.CheckEnemyInRange(targetLayer, direction, range, out enemies);
        
        public void Initialize(MonsterStat stat, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            var monsterTransform = transform;
            CreatureHpPanelUI = monsterHpUI;
            
            AnimatorSystem = GetComponent<AnimatorSystem>();
            CreatureCollider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            // TODO : MonsterStatData 캐릭터처럼 구조 수정
            _monsterStatsSystem = new MonsterStatSystem(stat);
            _monsterBattleSystem = new MonsterBattleSystem(_monsterStatsSystem, monsterTransform);
            _monsterHealthSystem = new MonsterHealthSystem(_monsterStatsSystem);
            _monsterMovementSystem = new MonsterMovementSystem(_monsterStatsSystem, monsterTransform, groundYPosition);

            AnimatorSystem.Initialize(animationParameter);
            FsmSystem = StateBuilder.BuildMonsterStateMachine(stateData, this, animationParameter, monsterTransform, _monsterStatsSystem);
        }
        
        public void ResetMonster()
        {
            FsmSystem.TryChangeState(StateType.Run);
            _spriteRenderer.color = Color.white;
            CreatureCollider.enabled = true;
            
            RegisterEventHandler();
            SetActiveHealthBarUI(true);
            
            _monsterStatsSystem.InitializeStat();
        }
        
        private void Update()
        {
            FsmSystem?.Update();
            _monsterMovementSystem?.Update();
            _monsterBattleSystem?.Update();
        }
        
        private void FixedUpdate()
        {
            FsmSystem?.FixedUpdate();
            _monsterMovementSystem?.FixedUpdate();
        }

        protected override void RegisterEventHandler()
        {
            _monsterStatsSystem.RegisterHandleOnDeath(HandleOnDeath);
            _monsterStatsSystem.RegisterHandleOnHit(HandleOnHit);
            _monsterStatsSystem.RegisterHandleOnUpdateHpPanelUI(HandleOnUpdateHpPanel);
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
            _monsterMovementSystem.SetImpact(1);
        }

        protected override void HandleOnDeath()
        {
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

        public void SetBool(int parameter, bool value, Action action)
        {
            AnimatorSystem.SetBool(parameter, value, action);
        }

        public void SetTrigger(int parameter, Action action)
        {
            AnimatorSystem.SetTrigger(parameter, action);
        }

        public void SetFloat(int parameter, int value, Action action)
        {
            AnimatorSystem.SetFloat(parameter, value, action);
        }

        public void AttackEnemy(RaycastHit2D target)
        {
            _monsterBattleSystem.AttackEnemy(GetDamage(), target);
        }

        public void TakeDamage(int value, Action<int> onIncreaseExp)
        {
            _monsterStatsSystem.RegisterHandleOnIncreasePlayerExp(onIncreaseExp);
            _monsterStatsSystem.HandleOnUpdateStat(StatType.CurrentHp, value * -1);
        }
        
        private void SetActiveHealthBarUI(bool active)
        {
            monsterHpPanel.gameObject.SetActive(active);
        }
    }
}