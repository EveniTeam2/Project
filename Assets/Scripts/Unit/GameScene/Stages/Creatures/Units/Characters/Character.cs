using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Boards.Blocks.Enums;
using Unit.GameScene.Boards.Interfaces;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules.Unit.Character;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Modules;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Units.CharacterSkills;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters
{
    public class Character : Creature
    {
        [SerializeField] protected CharacterType characterType;
        [SerializeField] private StateMachineDTO stateData;

        private CharacterServiceProvider _characterServiceProvider;
        private Queue<CommandPacket> _commands = new();
        private CommandSystem _commandSystem;
        private Stat<CharacterStat> _stats;
        private Dictionary<AnimationParameterEnums, int> _animationParameter;
        private List<CommandAction> _characterSkills;
        
        public void HandleReceiveCommand(CommandPacket command)
        {
            _commands.Enqueue(command);
        }

        public void Initialize(CharacterSetting characterSetting, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            var characterTransform = transform;
            characterType = characterSetting.Type;
            _animationParameter = animationParameter;
            
            _animator = GetComponent<Animator>();
            
            _stats = new Stat<CharacterStat>(characterSetting.Stat);
            _battleSystem = new CharacterBattleSystem(gameObject.transform, new CharacterBattleStat(_stats));
            _healthSystem = new CharacterHealthSystem(new CharacterHealthStat(_stats));
            _movementSystem = new CharacterMovementSystem(characterTransform, new CharacterMovementStat(_stats), groundYPosition);
            _fsm = StateBuilder.BuildState(stateData, characterTransform, _battleSystem, _healthSystem, _movementSystem, _animator, animationParameter);
            _characterServiceProvider = new CharacterServiceProvider(characterType, _battleSystem, _healthSystem, _movementSystem, _animator, _fsm, _animationParameter, characterSetting.CharacterSkillIndexes);
            _characterSkills = new CharacterSkillFactory(_characterServiceProvider, characterSetting.Type, characterSetting.CharacterSkillPresets).CreateSkill();
            _commandSystem = new CommandSystem(_characterServiceProvider, characterSetting.Type, _characterSkills);
            _mods = new LinkedList<ModifyStatData>();
            
            InitializeCommand();
        }
        
        protected void Update()
        {
            _fsm?.Update();
            _movementSystem?.Update();
            
            // TODO : 커맨드 인풋 내부로 이동 예정
            UpdateCommand();
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

        private void UpdateCommand()
        {
            if (_commands.Count > 0)
            {
                //Debug.Log("커맨드 Dequeue");

                var command = _commands.Dequeue();
                _commandSystem.ActivateCommand(command.BlockType, command.ComboCount);
                
                // TODO : 작업 예정
                
                // _commands.Dequeue().Execute(this);
            }
        }

        // public void Input(BlockType blockType, int count)
        // {
        //     _commandInput.Input(blockType, count);
        // }

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
            var cur = _stats.Current;
            switch (statType)
            {
                case EStatType.None:
                    break;
                case EStatType.Health:
                    cur.Health += value;
                    break;
                case EStatType.Attack:
                    cur.Attack += value;
                    break;
                case EStatType.Speed:
                    cur.Speed += value;
                    break;
            }

            _stats.SetCurrent(cur);
        }
    }
}