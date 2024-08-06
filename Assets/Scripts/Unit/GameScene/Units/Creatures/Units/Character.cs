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
using UnityEngine.UI;

namespace Unit.GameScene.Units.Creatures.Units
{
    public class Character : Creature, ICharacterFsmController, ICharacterSkillController, ITakeMonsterDamage, IUpdateCreatureStat
    {
        public event Action OnPlayerDeath;
        public event Action OnPlayerLevelUp;

        [SerializeField] protected CharacterClassType characterClassType;
        [SerializeField] private CharacterStateMachineDto characterStateMachineDto;

        protected override AnimatorSystem AnimatorSystem { get; set; }
        protected override Collider2D CreatureCollider { get; set; }
        protected override RectTransform CreatureHpHandler { get; set; }
        protected override RectMask2D CreatureHpHandlerMask { get; set; }
        
        private RectTransform _creatureExpHandler;
        private RectMask2D _creatureExpHandlerMask;
        private TextMeshProUGUI _characterLevelHandler;
        
        private CharacterData _characterData;

        private CharacterCommandSystem _characterCommandSystem;
        private CharacterSkillSystem _characterSkillSystem;

        private CharacterBattleSystem _characterBattleSystem;
        private CharacterHealthSystem _characterHealthSystem;
        private CharacterMovementSystem _characterMovementSystem;
        private CharacterStatSystem _characterStatSystem;
        
        private readonly Queue<CommandPacket> _commandQueue = new();

        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float range, out RaycastHit2D[] enemies) => _characterBattleSystem.CheckEnemyInRange(targetLayer, direction, range, out enemies);

        public void Initialize(CharacterData characterData, float groundYPosition, RectTransform characterHpHandler, RectTransform characterExpHandler, TextMeshProUGUI characterLevelHandler, Dictionary<AnimationParameterEnums, int> animationParameters, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            var characterTransform = transform;
            
            CreatureCollider = GetComponent<Collider2D>();
            AnimatorSystem = GetComponent<AnimatorSystem>();
            
            _characterData = characterData;
            characterClassType = _characterData.CharacterDataSo.classType;
            
            CreatureHpHandler = characterHpHandler;
            CreatureHpHandlerMask = CreatureHpHandler.GetComponent<RectMask2D>();
            _creatureExpHandler = characterExpHandler;
            _creatureExpHandlerMask = characterExpHandler.GetComponent<RectMask2D>();
            
            _characterLevelHandler = characterLevelHandler;
            AnimationParameters = animationParameters;
            
            _characterStatSystem = characterData.StatSystem;
            _characterSkillSystem = characterData.SkillSystem;
            _characterBattleSystem = new CharacterBattleSystem(_characterStatSystem, characterTransform);
            _characterMovementSystem = new CharacterMovementSystem(_characterStatSystem, characterTransform, groundYPosition);
            _characterHealthSystem = new CharacterHealthSystem(_characterStatSystem, _characterMovementSystem);
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
            _characterBattleSystem.PlayerAttackEnemy(value, range);
        }

        public void TakeDamage(int value)
        {
            if (AnimatorSystem.GetBool(AnimationParameterEnums.IsDead)) return;
            
            _characterMovementSystem.SetImpact();
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
            Debug.Log($"currentHp {currentExp} / maxHp {maxExp}");
            
            // 계산된 체력 비율
            float expRatio = (float)currentExp / maxExp;
    
            // Right 패딩을 조절하여 체력 바의 길이 조절
            float rightPadding = _creatureExpHandler.rect.width * (1 - expRatio);
            _creatureExpHandlerMask.padding = new Vector4(0, 0, rightPadding, 0);
        }

        private void HandleOnUpdateLevelPanelUI(int currentLevel)
        {
            _characterLevelHandler.text = "Lv." + $"{currentLevel + 1}";
        }
    }
}