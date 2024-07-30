using System;
using System.Collections.Generic;
using Unit.GameScene.Module;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules;

namespace Unit.GameScene.Units.Creatures.Module.Systems
{
    /// <summary>
    ///     사용자 입력을 처리하는 클래스입니다.
    /// </summary>
    public class CharacterCommandSystem : ICharacterCommand
    {
        private readonly Action _onCommandDequeue;
        private readonly Dictionary<BlockType, CharacterSkill> _skillCommands;
        
        private CommandAction _commandAction; 
        private Queue<CommandPacket> _commands;
        
        private bool _isReadyForCommand;

        /// <summary>
        ///     입력을 처리하는 클래스입니다.
        /// </summary>
        public CharacterCommandSystem(Dictionary<BlockType, CharacterSkill> blockInfo, Queue<CommandPacket> commands, Action onCommandDequeue)
        {
            _skillCommands = blockInfo;
            _commands = commands;
            _onCommandDequeue = onCommandDequeue;

            Initialize();
        }
        
        private void Initialize()
        {
            if (_commands == null) _commands = new Queue<CommandPacket>();
            else _commands.Clear();
        }
        
        public void Update()
        {
            if (!GetReadyForCommand() || _commands.Count <= 0) return;
            
            SetReadyForInvokingCommand(false);

            var command = _commands.Dequeue();
            _commandAction = new CommandAction(_skillCommands.GetValueOrDefault(command.BlockType));
            
            
            if (!ActivateCommand(command.BlockType, command.ComboCount))
            {
                SetReadyForInvokingCommand(true);
            }
            
            _onCommandDequeue?.Invoke();
        }
        
        /// <summary>
        ///     입력을 처리합니다.
        /// </summary>
        private bool ActivateCommand(BlockType blockType, int count)
        {
            var command = _skillCommands[blockType];

            if (command == null) return false;
            
            command.Execute(count);
            return true;
        }
        
        public void ActivateSkillEffects()
        {
            _commandAction?.ActivateCommandAction();
        }

        private bool GetReadyForCommand()
        {
            return _isReadyForCommand;
        }

        public void SetReadyForInvokingCommand(bool value)
        {
            _isReadyForCommand = value;
        }
    }
}