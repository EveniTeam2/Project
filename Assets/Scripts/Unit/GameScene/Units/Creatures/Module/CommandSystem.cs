using System;
using System.Collections.Generic;
using System.Windows.Input;
using Unit.GameScene.Units.Blocks.Units.MatchBlock.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module
{
    /// <summary>
    ///     사용자 입력을 처리하는 클래스입니다.
    /// </summary>
    public class CommandSystem
    {
        private readonly Dictionary<BlockType, CharacterSkill> _skillCommands;

        /// <summary>
        ///     입력을 처리하는 클래스입니다.
        /// </summary>
        public CommandSystem(Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            _skillCommands = blockInfo;
        }
        
        public CommandAction GetCommandAction(BlockType blockType)
        {
            return new CommandAction(_skillCommands.GetValueOrDefault(blockType));
        }
        
        /// <summary>
        ///     입력을 처리합니다.
        /// </summary>
        public bool ActivateCommand(BlockType blockType, int count)
        {
            var command = _skillCommands[blockType];

            if (command == null) return false;
            
            command.Execute(count);
            return true;

        }
    }
}