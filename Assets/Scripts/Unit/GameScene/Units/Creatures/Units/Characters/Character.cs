using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Units.Blocks.Units.MatchBlock.Enums;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.FSM;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters
{
    public class Character : Creature, ICharacterServiceProvider
    {
        public event Action OnCommandDequeue;
        
        [SerializeField] protected CharacterClassType characterClassType;
        [SerializeField] private StateMachineDTO stateData;

        private CharacterData _characterData;
        private Queue<CommandPacket> _commands = new();
        private CommandSystem _commandSystem;
        private CommandAction _commandAction; 
        private CreatureStat<CharacterStat> _creatureStats;
        private List<CommandAction> _characterSkills;

        private bool isReadyForCommand;

        private CharacterBattleSystem _battleSystem;
        private CharacterHealthSystem _healthSystem;
        private CharacterMovementSystem _movementSystem;

        protected override BattleSystem BattleSystem => _battleSystem;
        protected override HealthSystem HealthSystem => _healthSystem;
        protected override MovementSystem MovementSystem => _movementSystem;

        ICharacterBattle ICharacterServiceProvider.BattleSystem => _battleSystem;

        ICharacterHealth ICharacterServiceProvider.HeathSystem => _healthSystem;

        ICharacterMovement ICharacterServiceProvider.MovementSystem => _movementSystem;

        ICreatureServiceProvider ICharacterServiceProvider.creatureServiceProvider => throw new NotImplementedException();

        public event Action OnLevelUP;

        public void HandleReceiveCommand(CommandPacket command)
        {
            _commands.Enqueue(command);
        }

        public void Initialize(CharacterData characterData, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameter, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            var characterTransform = transform;
            _characterData = characterData;
            characterClassType = _characterData.CharacterDataSo.classType;
            _animatorEventReceiver = GetComponent<AnimatorEventReceiver>();
            _animatorEventReceiver.Initialize(animationParameter);
            _animatorEventReceiver.OnAttack += ActivateSkillEffects;
            _creatureStats = new CreatureStat<CharacterStat>(GetCharacterStat(1));
            _battleSystem = new CharacterBattleSystem(gameObject.transform, new CharacterBattleStat(_creatureStats, _characterData));
            _healthSystem = new CharacterHealthSystem(new CharacterHealthStat(_creatureStats));
            _movementSystem = new CharacterMovementSystem(characterTransform, new CharacterMovementStat(_creatureStats), groundYPosition);
            _fsm = StateBuilder.BuildStateMachine(stateData, characterTransform, BattleSystem, HealthSystem, MovementSystem, _animatorEventReceiver, animationParameter);
            _commandSystem = new CommandSystem(blockInfo);
            _mods = new LinkedList<ModifyStatData>();
            
            InitializeCommand();
            
            _characterData.RegisterCharacterServiceProvider(this);
            HealthSystem.RegistOnDamageEvent(CheckAndTransitToHit);
        }
        
        private CharacterStat GetCharacterStat(int currentLevel)
        {
            var statManager = _characterData.StatManager;

            return new CharacterStat(
                statManager.GetMaxHp(currentLevel),
                statManager.GetMaxShield(currentLevel),
                statManager.GetDamage(currentLevel),
                statManager.GetSpeed(currentLevel),
                statManager.GetCardTrigger(currentLevel),
                statManager.GetMaxExp(currentLevel));
        }

        private void CheckAndTransitToHit()
        {
            var stateType = _fsm.GetCurrentStateType();
            if (stateType == StateType.Idle || stateType == StateType.Run)
            {
                _fsm.TryChangeState(StateType.Hit);
            }
        }

        protected void Update()
        {
            _fsm?.Update();
            MovementSystem?.Update();
            BattleSystem?.Update();
            // TODO : 커맨드 인풋 내부로 이동 예정
            ActivateCommand();
        }
        
        protected void FixedUpdate()
        {
            _fsm?.FixedUpdate();
            MovementSystem?.FixedUpdate();
        }
        
        private void InitializeCommand()
        {
            if (_commands == null) _commands = new Queue<CommandPacket>();
            else _commands.Clear();
        }

        private void ActivateCommand()
        {
            if (!GetReadyForCommand() || _commands.Count <= 0) return;
            
            SetReadyForCommand(false);

            var command = _commands.Dequeue();
            _commandAction = _commandSystem.GetCommandAction(command.BlockType);
            
            if (!_commandSystem.ActivateCommand(command.BlockType, command.ComboCount))
            {
                SetReadyForCommand(true);
            }
            
            OnCommandDequeue?.Invoke();
        }

        public void ActivateSkillEffects()
        {
            _commandAction?.ActivateCommandAction();
        }



        public override void PermanentModifyStat(EStatType statType, int value)
        {
            _mods.AddLast(new ModifyStatData(statType, value));
            ModifyStat(statType, value);
        }

        public override void TempModifyStat(EStatType statType, int value, float duration)
        {
            StartCoroutine(TempModifyStatCoroutine(statType, value, duration));
        }

        public override void ClearModifiedStat()
        {
            _mods.Clear();
            _creatureStats.SetCurrent(_creatureStats.Origin);
        }

        protected override void ModifyStat(EStatType statType, int value)
        {
            // TODO : 채이환
            var cur = _creatureStats.Current;
            switch (statType)
            {
                case EStatType.None:
                    break;
                case EStatType.Health:
                    cur.CurrentHp += value;
                    break;
                case EStatType.Attack:
                    cur.Damage += value;
                    break;
                case EStatType.Speed:
                    cur.Speed += value;
                    break;
            }

            _creatureStats.SetCurrent(cur);
        }

        public bool GetReadyForCommand()
        {
            return isReadyForCommand;

        }

        public void SetReadyForCommand(bool value)
        {
            isReadyForCommand = value;
        }

    }
}