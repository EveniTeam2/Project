using ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs;
using System;
using System.Collections.Generic;
using TMPro;
using Unit.GameScene.Module;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Cards.Interfaces;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Data.CharacterDatas;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units
{
    public class Character : Creature, ICharacterFsmController, ICharacterSkillController, ITakeMonsterDamage, IUpdateCreatureStat
    {
        public event Action OnPlayerDeath;
        public event Action OnPlayerLevelUp;

        [SerializeField] protected CharacterClassType characterClassType;
        [SerializeField] private CharacterStateMachineDto characterStateMachineDto;

        protected override Collider2D CreatureCollider { get; set; }
        protected override RectTransform CreatureHpPanelUI { get; set; }
        protected override AnimatorSystem AnimatorSystem { get; set; }
        
        private readonly Queue<CommandPacket> _commandQueue = new();

        private CharacterData _characterData;

        private CharacterCommandSystem _characterCommandSystem;
        private CharacterSkillSystem _characterSkillSystem;

        private CharacterBattleSystem _characterBattleSystem;
        private CharacterHealthSystem _characterHealthSystem;
        private CharacterMovementSystem _characterMovementSystem;
        private CharacterStatSystem _characterStatSystem;

        private RectTransform _characterExpPanelUI;
        private TextMeshProUGUI _characterLevelPanelUI;

        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float range, out RaycastHit2D[] enemies) => _characterBattleSystem.CheckEnemyInRange(targetLayer, direction, range, out enemies);

        public void Initialize(CharacterData characterData, float groundYPosition, RectTransform characterHpPanelUI, RectTransform characterExpPanelUI, TextMeshProUGUI characterLevelPanelUI, Dictionary<AnimationParameterEnums, int> animationParameters, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            var characterTransform = transform;
            
            CreatureCollider = GetComponent<Collider2D>();
            AnimatorSystem = GetComponent<AnimatorSystem>();
            
            _characterData = characterData;
            characterClassType = _characterData.CharacterDataSo.classType;
            CreatureHpPanelUI = characterHpPanelUI;
            _characterExpPanelUI = characterExpPanelUI;
            _characterLevelPanelUI = characterLevelPanelUI;
            AnimationParameters = animationParameters;
            
            _characterStatSystem = characterData.StatSystem;
            _characterSkillSystem = characterData.SkillSystem;
            _characterBattleSystem = new CharacterBattleSystem(_characterStatSystem, characterTransform);
            _characterHealthSystem = new CharacterHealthSystem(_characterStatSystem);
            _characterMovementSystem = new CharacterMovementSystem(_characterStatSystem, characterTransform, groundYPosition);
            _characterCommandSystem = new CharacterCommandSystem(blockInfo, _commandQueue);

            AnimatorSystem.Initialize(AnimationParameters);
            AnimatorSystem.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], false, null);
            
            FsmSystem = StateBuilder.BuildCharacterStateMachine(characterStateMachineDto, this, AnimatorSystem, AnimationParameters);

            RegisterEventHandler();

            _characterData.RegisterCharacterServiceProvider(this);
            _characterStatSystem.InitializeStat();
        }

        protected void Update()
        {
            if (AnimatorSystem.GetBool(AnimationParameterEnums.IsDead)) return;
            
            FsmSystem?.Update();
            _characterMovementSystem?.Update();

            if (FsmSystem?.GetCurrentStateType() is StateType.Idle or StateType.Run)
            {
                _characterCommandSystem?.Update();
            }
        }

        protected void FixedUpdate()
        {
            if (AnimatorSystem.GetBool(AnimationParameterEnums.IsDead)) return;
            
            FsmSystem?.FixedUpdate();
            _characterMovementSystem?.FixedUpdate();
        }

        public void ToggleMovement(bool setRunning)
        {
            _characterMovementSystem.SetRun(setRunning);
        }

        public void HealMySelf(int value)
        {
            _characterHealthSystem.TakeHeal(value);
        }

        public void SetReadyForInvokingCommand(bool isReady)
        {
            _characterCommandSystem.SetReadyForInvokingCommand(isReady);
        }

        public void TryChangeState(StateType targetState)
        {
            FsmSystem.TryChangeState(targetState);
        }

        public void Summon()
        {
            // TODO : 소환 스킬이 추가되면 고쳐야함
        }

        public void AttackEnemy(int value, float range)
        {
            // TODO : Range 더 늘려야 할 것 같음
            _characterBattleSystem.AttackEnemy(value, range * 10);
        }

        public void TakeDamage(int value)
        {
            if (AnimatorSystem.GetBool(AnimationParameterEnums.IsDead)) return;
            
            _characterStatSystem.HandleOnUpdateStat(StatType.CurrentHp, value * -1);
        }
        
        public void UpdateCreatureStat(StatType statType, float value)
        {
            _characterStatSystem.HandleOnUpdateStat(statType, value);
        }
        
        protected override void RegisterEventHandler()
        {
            OnUpdateStat += _characterStatSystem.HandleOnUpdateStat;
            AnimatorSystem.OnAttack += _characterCommandSystem.ActivateSkillEffects;

            _characterStatSystem.RegisterHandleOnDeath(HandleOnDeath);
            _characterStatSystem.RegisterHandleOnHit(HandleOnHit);
            _characterStatSystem.RegisterHandleOnUpdateHpPanelUI(HandleOnUpdateHpPanel);
            _characterStatSystem.RegisterHandleOnUpdateExpPanelUI(HandleOnUpdateExpPanelUI);
            _characterStatSystem.RegisterHandleOnUpdateLevelPanelUI(HandleOnUpdateLevelPanelUI);

            FsmSystem.RegisterHandleOnDeathState(HandleOnGameOver);
        }
        
        public void RegisterHandleOnCommandDequeue(Action action)
        {
            _characterCommandSystem.OnCommandDequeue += action;
        }

        public void RegisterHandleOnPlayerDeath(Action action)
        {
            OnPlayerDeath += action;
        }

        protected override void HandleOnHit()
        {
            var stateType = FsmSystem.GetCurrentStateType();

            if (stateType is StateType.Skill or StateType.Hit or StateType.Die)
            {
                return;
            }

            FsmSystem.TryChangeState(StateType.Hit);
            _characterMovementSystem.SetImpact(1);
        }

        protected override void HandleOnDeath()
        {
            AnimatorSystem.SetBool(AnimationParameterEnums.IsDead, true, null);
            SetActiveCollider(false);
            // TODO : 채이환 - [Bug] FSM DieState로 전환되어 Die 파라미터가 트리거된 이후 간헐적으로 Idle 파라미터가 True로 바뀌고 있습니다. 수정해주세요. 
            FsmSystem.TryChangeState(StateType.Die);
        }
        
        public void RegisterOnHandleOnTriggerCard(Action action)
        {
            _characterStatSystem.RegisterHandleOnTriggerCard(action);
        }

        public void HandleOnSendCommand(CommandPacket command)
        {
            _commandQueue.Enqueue(command);
        }

        private void HandleOnGameOver()
        {
            OnPlayerDeath.Invoke();
        }

        private void HandleOnUpdateExpPanelUI(int currentExp, int maxExp)
        {
            Debug.Log($"currentExp {currentExp} / maxExp {maxExp}");
            // 계산된 체력 비율
            float expRatio = (float)currentExp / maxExp;
    
            // 새로운 localScale 값 계산
            var newScale = new Vector3(expRatio, CreatureHpPanelUI.localScale.y, CreatureHpPanelUI.localScale.z);
    
            // 체력 바의 스케일을 업데이트
            _characterExpPanelUI.localScale = newScale;
        }

        private void HandleOnUpdateLevelPanelUI(int currentLevel)
        {
            _characterLevelPanelUI.text = "Lv." + $"{currentLevel + 1}";
        }
    }
}