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
using Unit.GameScene.Units.Creatures.Interfaces.Battles;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Creatures.Units
{
    public class Character : Creature, ICharacterFsmController, ICharacterSkillController, ITakeMonsterDamage, IUpdateCreatureStat
    {
        public event Action OnPlayerDeath;

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

            // 시스템 초기화
            _characterStatSystem = characterData.StatSystem;
            _characterBattleSystem = new CharacterBattleSystem(_characterStatSystem, characterTransform);
            _characterMovementSystem = new CharacterMovementSystem(_characterStatSystem, characterTransform, groundYPosition);
            _characterHealthSystem = new CharacterHealthSystem(_characterMovementSystem, _characterStatSystem.RegisterHandleOnUpdatePermanentStat, _characterStatSystem.RegisterHandleOnUpdateTemporaryStat);
            _characterCommandSystem = new CharacterCommandSystem(blockInfo, _commandQueue);

            AnimatorSystem.Initialize(AnimationParameters);
            AnimatorSystem.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], false, null);

            FsmSystem = StateBuilder.BuildCharacterStateMachine(characterStateMachineDto, this, AnimatorSystem, AnimationParameters);

            RegisterEventHandler();

            _characterData.RegisterCharacterServiceProvider(this);
            _characterStatSystem.InitializeStat(this);
        }

        private void Update()
        {
            if (AnimatorSystem.GetBool(AnimationParameterEnums.IsDead)) return;

            FsmSystem?.Update();
            _characterMovementSystem?.Update();

            if (FsmSystem?.GetCurrentStateType() is StateType.Idle or StateType.Run)
            {
                _characterCommandSystem?.Update();
            }
        }

        private void FixedUpdate()
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
            // TODO: 소환 스킬이 추가되면 수정 필요
        }

        public void AttackEnemy(int value, float range)
        {
            _characterBattleSystem.AttackEnemy(value, range);
        }

        public void AdjustBuffDamage(int value, float duration)
        {
            _characterStatSystem.RegisterHandleOnUpdateTemporaryStat(StatType.Damage, value, duration);
        }

        public void TakeDamage(int value)
        {
            if (AnimatorSystem.GetBool(AnimationParameterEnums.IsDead)) return;

            _characterMovementSystem.SetImpact();
            _characterStatSystem.RegisterHandleOnUpdatePermanentStat(StatType.CurrentHp, value * -1);
        }

        public void UpdateCreatureStat(StatType statType, float value)
        {
            _characterStatSystem.RegisterHandleOnUpdatePermanentStat(statType, value);
        }

        protected override void RegisterEventHandler()
        {
            AnimatorSystem.OnAttack += _characterCommandSystem.ActivateSkillEffects;

            _characterStatSystem.RegisterHandleOnDeath(HandleOnDeath);
            _characterStatSystem.RegisterHandleOnHit(HandleOnHit);
            _characterStatSystem.RegisterHandleOnUpdateHpPanelUI(UpdateHpBar);
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
        
        public void RegisterOnHandleOnTriggerCard(Action action)
        {
            _characterStatSystem.RegisterHandleOnTriggerCard(action);
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
            AnimatorSystem.SetBool(AnimationParameters[AnimationParameterEnums.IsDead], true, null);
            SetActiveCollider(false);

            FsmSystem.TryChangeState(StateType.Die);
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
