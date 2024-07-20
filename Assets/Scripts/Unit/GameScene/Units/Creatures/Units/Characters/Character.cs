using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Manager.Modules;
using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules.Unit.Character;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Modules;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Units.CharacterSkills;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters
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

        private bool _isSkillAnimationRunning;
        
        public void HandleReceiveCommand(CommandPacket command)
        {
            _commands.Enqueue(command);
        }

        public void Initialize(CharacterSetting characterSetting, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            _isSkillAnimationRunning = false;
            
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
            if (_isSkillAnimationRunning == false && _commands.Count > 0)
            {
                if (_fsm.GetCurrentStateType() == StateType.Idle || _fsm.GetCurrentStateType() == StateType.Run )
                {
                    Debug.Log($"현재 StateType : {_fsm.GetCurrentStateType()}");


                    var command = _commands.Dequeue();
                    _commandSystem.ActivateCommand(command.BlockType, command.ComboCount);
                }
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
        
        /// <summary>
        /// Character Animator 이벤트입니다.
        /// </summary>
        /// <param name="result"></param>
        public void CheckAnimatorRunning(int result)
        {
            _isSkillAnimationRunning = result == 1;
            Debug.Log($"애니메이션 동작 여부 {_isSkillAnimationRunning}");
        }
    }
}