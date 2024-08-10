using System;
using System.Collections.Generic;
using TMPro;
using Unit.GameScene.Module;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Cards.Interfaces;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Data.CharacterDatas;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;
using UnityEngine;
using UnityEngine.UI;
using ICharacterSkillController = Unit.GameScene.Units.Creatures.Interfaces.Battles.ICharacterSkillController;

namespace Unit.GameScene.Units.Creatures.Units
{
    public class Character : Creature, ICharacterSkillController, ICardController
    {
        public event Action OnPlayerDeath;

        [SerializeField] protected CharacterType characterType;

        protected override AnimationEventReceiver AnimationEventReceiver { get; set; }
        protected override Collider2D CreatureCollider { get; set; }
        protected override RectTransform CreatureHpHandler { get; set; }
        protected override RectMask2D CreatureHpHandlerMask { get; set; }

        private RectTransform _creatureExpHandler;
        private RectMask2D _creatureExpHandlerMask;
        private TextMeshProUGUI _characterLevelHandler;

        private CharacterData _characterData;

        private CharacterSkillSystem _characterSkillSystem;
        private CharacterCommandSystem _characterCommandSystem;
        private CharacterBattleSystem _characterBattleSystem;
        private CharacterMovementSystem _characterMovementSystem;
        private CharacterStatSystem _characterStatSystem;

        private readonly Queue<CommandPacket> _commandQueue = new();

        public bool CheckEnemyInRange(float range, out RaycastHit2D[] enemies) => _characterBattleSystem.CheckEnemyInRange(range, out enemies);

        public void Initialize(CharacterData characterData, float groundYPosition, RectTransform characterHpHandler, RectTransform characterExpHandler, TextMeshProUGUI characterLevelHandler, Dictionary<AnimationParameterEnums, int> animationParameters, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            var characterTransform = transform;

            CreatureCollider = GetComponent<Collider2D>();
            AnimationEventReceiver = GetComponent<AnimationEventReceiver>();

            _characterData = characterData;
            characterType = _characterData.CharacterDataSo.type;
            
            AnimationParameters = animationParameters;

            // 시스템 초기화
            _characterStatSystem = characterData.StatSystem;
            _characterSkillSystem = characterData.SkillSystem;
            _characterBattleSystem = new CharacterBattleSystem(_characterStatSystem, characterTransform);
            _characterMovementSystem = new CharacterMovementSystem(_characterStatSystem, characterTransform, groundYPosition);
            _characterCommandSystem = new CharacterCommandSystem(blockInfo, _commandQueue);

            AnimationEventReceiver.Initialize(AnimationParameters);
            AnimationEventReceiver.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], false, null);

            RegisterEventHandler(characterHpHandler, characterExpHandler, characterLevelHandler);
            
            CreatureHpHandlerMask = CreatureHpHandler.GetComponent<RectMask2D>();
            _creatureExpHandlerMask = characterExpHandler.GetComponent<RectMask2D>();

            _characterData.RegisterCharacterServiceProvider(this);
            _characterStatSystem.InitializeStat(this);
            
            StateMachine = new CharacterStateMachine(_characterSkillSystem.GetDefaultSkill().GetSkillRange1(), AnimationEventReceiver, _characterStatSystem, _characterMovementSystem, _characterBattleSystem, _characterSkillSystem);
            StateMachine.ChangeState(StateType.Run);
        }

        private void RegisterEventHandler(RectTransform characterHpHandler, RectTransform characterExpHandler, TextMeshProUGUI characterLevelHandler)
        {
            CreatureHpHandler = characterHpHandler;
            _creatureExpHandler = characterExpHandler;
            _characterLevelHandler = characterLevelHandler;
            
            AnimationEventReceiver.OnActivateSkillEffect += _characterCommandSystem.ActivateSkillEffects;

            _characterStatSystem.RegisterHandleOnDeath(HandleOnDeath);
            _characterStatSystem.RegisterHandleOnHit(HandleOnHit);
            _characterStatSystem.RegisterHandleOnUpdateHpPanelUI(UpdateHpBar);
            _characterStatSystem.RegisterHandleOnUpdateExpPanelUI(HandleOnUpdateExpPanelUI);
            _characterStatSystem.RegisterHandleOnUpdateLevelPanelUI(HandleOnUpdateLevelPanelUI);
        }

        private void Update()
        {
            if (AnimationEventReceiver.GetBool(AnimationParameterEnums.IsDead)) return;

            StateMachine?.Update();
            _characterMovementSystem?.Update();

            if (StateMachine?.GetCurrentStateType() is StateType.Idle or StateType.Run)
            {
                _characterCommandSystem?.Update();
            }
        }

        private void FixedUpdate()
        {
            if (AnimationEventReceiver.GetBool(AnimationParameterEnums.IsDead)) return;
            
            _characterMovementSystem?.FixedUpdate();
        }

        public void ToggleMovement(bool setRunning)
        {
            _characterMovementSystem.SetRun(setRunning);
        }

        public void HealMySelf(int value)
        {
            _characterStatSystem.RegisterHandleOnUpdatePermanentStat(StatType.CurrentHp, value);
        }

        public void SetReadyForInvokingCommand(bool isReady)
        {
            _characterCommandSystem.SetReadyForInvokingCommand(isReady);
        }

        public void TryChangeState(StateType targetState)
        {
            StateMachine.ChangeState(targetState);
        }

        public void Attack(int value, float range)
        {
            _characterBattleSystem.AttackEnemy(value, range);
        }

        public void Heal(int value)
        {
            _characterStatSystem.RegisterHandleOnUpdatePermanentStat(StatType.CurrentHp, value);
        }

        public void Buff(StatType statType, int value, float duration)
        {
            _characterStatSystem.RegisterHandleOnUpdateTemporaryStat(StatType.CurrentHp, value, duration);
        }

        public void Summon()
        {
            // TODO: 소환 스킬이 추가되면 수정 필요
        }

        public void TakeDamage(int value)
        {
            if (AnimationEventReceiver.GetBool(AnimationParameterEnums.IsDead)) return;

            _characterMovementSystem.SetImpact();
            _characterStatSystem.RegisterHandleOnUpdatePermanentStat(StatType.CurrentHp, value * -1);
        }

        public void UpdateCreatureStat(StatType statType, float value)
        {
            _characterStatSystem.RegisterHandleOnUpdatePermanentStat(statType, value);
        }

        public void RegisterHandleOnCommandDequeue(Action action)
        {
            _characterCommandSystem.OnCommandDequeue += action;
        }

        public void RegisterHandleOnPlayerDeath(Action action)
        {
            OnPlayerDeath += action;
        }
        
        public void RegisterOnHandleOnTriggerCard(Action action)
        {
            _characterStatSystem.RegisterHandleOnTriggerCard(action);
        }

        protected override void HandleOnHit()
        {
            var stateType = StateMachine.GetCurrentStateType();

            if (stateType is StateType.Skill or StateType.Hit or StateType.Die)
            {
                return;
            }

            StateMachine.ChangeState(StateType.Hit);
        }

        protected override void HandleOnDeath()
        {
            AnimationEventReceiver.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], true, null);
            SetActiveCollider(false);

            StateMachine.ChangeState(StateType.Die);
        }

        public void HandleOnSendCommand(CommandPacket command)
        {
            _commandQueue.Enqueue(command);
        }

        private void HandleOnGameOver()
        {
            OnPlayerDeath?.Invoke();
        }

        private void HandleOnUpdateExpPanelUI(int currentExp, int maxExp)
        {
            Debug.Log($"currentExp {currentExp} / maxExp {maxExp}");

            float expRatio = (float)currentExp / maxExp;

            float rightPadding = _creatureExpHandler.rect.width * (1 - expRatio);
            _creatureExpHandlerMask.padding = new Vector4(0, 0, rightPadding, 0);
        }

        private void HandleOnUpdateLevelPanelUI(int currentLevel)
        {
            _characterLevelHandler.text = "Lv." + $"{currentLevel + 1}";
        }
    }
}
