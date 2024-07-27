using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Units.Blocks.Units.MatchBlock.Enums;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;

using Unit.GameScene.Units.Creatures.Units.FSM;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters
{
    public class Character : Creature
    {
        public event Action OnCommandDequeue;
        
        [SerializeField] protected CharacterClassType characterClassType;
        [SerializeField] private StateMachineDTO stateData;

        private CharacterData _characterData;
        private Queue<CommandPacket> _commands = new();
        private CharacterServiceProvider _characterServiceProvider;
        private CommandSystem _commandSystem;
        private CommandAction _commandAction; 
        private Stat<CharacterStat> _stats;
        private Dictionary<AnimationParameterEnums, int> _animationParameter;
        private List<CommandAction> _characterSkills;
        
        public void HandleReceiveCommand(CommandPacket command)
        {
            _commands.Enqueue(command);
        }

        public void Initialize(CharacterData characterData, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameter, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            var characterTransform = transform;
            _characterData = characterData;
            characterClassType = _characterData.CharacterDataSo.classType;
            _animationParameter = animationParameter;
            
            _animatorEventReceiver = GetComponent<AnimatorEventReceiver>();
            _animatorEventReceiver.OnAttack += ActivateSkill;
            
            _stats = new Stat<CharacterStat>(GetCharacterStat(1));
            _battleSystem = new CharacterBattleSystem(gameObject.transform, new CharacterBattleStat(_stats));
            _healthSystem = new CharacterHealthSystem(new CharacterHealthStat(_stats));
            _movementSystem = new CharacterMovementSystem(characterTransform, new CharacterMovementStat(_stats), groundYPosition);
            _fsm = StateBuilder.BuildStateMachine(stateData, characterTransform, _battleSystem, _healthSystem, _movementSystem, _animatorEventReceiver, animationParameter);
            _characterServiceProvider = new CharacterServiceProvider(characterClassType, _battleSystem, _healthSystem, _movementSystem, _animatorEventReceiver, _fsm, _animationParameter, characterData);
            _commandSystem = new CommandSystem(blockInfo);
            _mods = new LinkedList<ModifyStatData>();
            
            InitializeCommand();
            
            _characterData.RegisterCharacterServiceProvider(_characterServiceProvider);
            _healthSystem.RegistOnDamageEvent(CheckAndTransitToHit);
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
            _movementSystem?.Update();
            
            // TODO : 커맨드 인풋 내부로 이동 예정
            ActivateCommand();
        }
        
        protected void FixedUpdate()
        {
            _fsm?.FixedUpdate();
            _movementSystem?.FixedUpdate();
        }
        
        private void InitializeCommand()
        {
            if (_commands == null) _commands = new Queue<CommandPacket>();
            else _commands.Clear();
        }

        private void ActivateCommand()
        {
            if (!_characterServiceProvider.GetReadyForCommand() || _commands.Count <= 0) return;
            
            _characterServiceProvider.SetReadyForCommand(false);

            var command = _commands.Dequeue();
            _commandAction = _commandSystem.GetCommandAction(command.BlockType);
            
            if (!_commandSystem.ActivateCommand(command.BlockType, command.ComboCount))
            {
                _characterServiceProvider.SetReadyForCommand(true);
            }
            
            OnCommandDequeue?.Invoke();
        }

        private void ActivateSkill()
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
            _stats.SetCurrent(_stats.Origin);
        }

        public CharacterServiceProvider GetServiceProvider()
        {
            return _characterServiceProvider;
        }

        protected override void ModifyStat(EStatType statType, int value)
        {
            // TODO : 채이환
            var cur = _stats.Current;
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

            _stats.SetCurrent(cur);
        }
    }
}