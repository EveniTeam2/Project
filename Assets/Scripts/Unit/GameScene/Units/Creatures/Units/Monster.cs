using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Data.MonsterDatas;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units
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
        protected override AnimatorSystem AnimatorSystem { get; set; }

        private MonsterBattleSystem _monsterBattleSystem;
        private MonsterHealthSystem _monsterHealthSystem;
        private MonsterMovementSystem _monsterMovementSystem;
        private MonsterStatSystem _monsterStatsSystem;
        
        public int GetDamage() => _monsterStatsSystem.Damage;
        public bool IsReadyForAttack() => _monsterBattleSystem.IsReadyForAttack;
        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float range, out RaycastHit2D[] enemies) => _monsterBattleSystem.CheckEnemyInRange(targetLayer, direction, range, out enemies);
        
        public void Initialize(MonsterStat stat, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameters)
        {
            var monsterTransform = transform;
            CreatureHpPanelUI = monsterHpUI;
            AnimationParameters = animationParameters;
            
            AnimatorSystem = GetComponent<AnimatorSystem>();
            CreatureCollider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            // TODO : MonsterStatData 캐릭터처럼 구조 수정
            _monsterStatsSystem = new MonsterStatSystem(stat);
            _monsterBattleSystem = new MonsterBattleSystem(_monsterStatsSystem, monsterTransform);
            _monsterMovementSystem = new MonsterMovementSystem(_monsterStatsSystem, monsterTransform, groundYPosition);
            _monsterHealthSystem = new MonsterHealthSystem(_monsterStatsSystem, _monsterMovementSystem);

            AnimatorSystem.Initialize(AnimationParameters);
            
            FsmSystem = StateBuilder.BuildMonsterStateMachine(stateData, this, AnimationParameters, monsterTransform, _monsterStatsSystem);
        }
        
        public void ResetMonster()
        {
            AnimatorSystem.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], false, null);
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
        }

        protected override void HandleOnDeath()
        {
            AnimatorSystem.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], true, null);
            SetActiveHealthBarUI(false);
            SetActiveCollider(false);
            
            // TODO : 채이환 - [Bug] 몬스터가 풀에 릴리즈 되고 이후 다시 스폰되면 죽었을 때 사망 애니메이션이 스킵되고 바로 릴리즈 되는 것 같습니다.
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
            _monsterBattleSystem.AttackEnemy(GetDamage(), target);
        }
        
        public Func<int, int> TakeDamage()
        {
            return damage =>
            {
                _monsterStatsSystem.HandleOnUpdateStat(StatType.CurrentHp, damage * -1);

                return _monsterStatsSystem.ReturnExp();
            };
        }
        
        private void SetActiveHealthBarUI(bool active)
        {
            monsterHpPanel.gameObject.SetActive(active);
        }
    }
}