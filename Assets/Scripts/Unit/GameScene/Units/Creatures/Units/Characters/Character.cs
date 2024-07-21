using System.Collections.Generic;
using Core.Utils;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules.Unit.Character;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Units.CharacterSkills;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Units.Creatures.Units.Characters
{
    public class Character : Creature
    {
        [SerializeField] protected CharacterType characterType;
        [SerializeField] private StateMachineDTO stateData;
        [SerializeField] private SerializableQueue<CommandPacket> commands = new();
        
        private CharacterServiceProvider _characterServiceProvider;
        private CommandSystem _commandSystem;
        private Stat<CharacterStat> _stats;
        private Dictionary<AnimationParameterEnums, int> _animationParameter;
        private List<CommandAction> _characterSkills;
        
        public void HandleReceiveCommand(CommandPacket command)
        {
            commands.Enqueue(command);
        }

        public void Initialize(CharacterSetting characterSetting, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            var characterTransform = transform;
            characterType = characterSetting.Type;
            _animationParameter = animationParameter;
            
            // _animator = GetComponent<Animator>();
            _animatorEventReceiver = GetComponent<AnimatorEventReceiver>();
            
            _stats = new Stat<CharacterStat>(characterSetting.Stat);
            _battleSystem = new CharacterBattleSystem(gameObject.transform, new CharacterBattleStat(_stats));
            _healthSystem = new CharacterHealthSystem(new CharacterHealthStat(_stats));
            _movementSystem = new CharacterMovementSystem(characterTransform, new CharacterMovementStat(_stats), groundYPosition);
            _fsm = StateBuilder.BuildState(stateData, characterTransform, _battleSystem, _healthSystem, _movementSystem, _animatorEventReceiver, animationParameter);
            _characterServiceProvider = new CharacterServiceProvider(characterType, _battleSystem, _healthSystem, _movementSystem, _animatorEventReceiver, _fsm, _animationParameter, characterSetting.CharacterSkillIndexes, characterSetting.CharacterSkillValues);
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
            ActivateCommand();
        }
        
        protected void FixedUpdate()
        {
            _fsm?.FixedUpdate();
            _movementSystem?.FixedUpdate();
        }
        
        private void InitializeCommand()
        {
            if (commands == null) commands = new SerializableQueue<CommandPacket>();
            else commands.Clear();
        }

        private void ActivateCommand()
        {
            if (_characterServiceProvider.GetReadyForCommand() && commands.Count > 0)
            {
                Debug.Log($"ReadyForCommand : {_characterServiceProvider.GetReadyForCommand()}");
                _characterServiceProvider.SetReadyForCommand(false);

                Debug.Log($"ReadyForCommand : {_characterServiceProvider.GetReadyForCommand()}");

                var command = commands.Dequeue();

                Debug.Log($"{command.ComboCount} 콤보 {command.BlockType} 블록 커맨드 Dequeue");
                Debug.Log($"잔여 커맨드 : {commands.Count}");
                
                _commandSystem.ActivateCommand(command.BlockType, command.ComboCount);
            }
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