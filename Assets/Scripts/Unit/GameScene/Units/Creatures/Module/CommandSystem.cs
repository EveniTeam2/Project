using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Blocks.Units.MatchBlock.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module
{
    /// <summary>
    ///     사용자 입력을 처리하는 클래스입니다.
    /// </summary>
    public class CommandSystem
    {
        private readonly ICreatureServiceProvider _serviceProvider;
        private readonly Dictionary<BlockType, CommandAction> _skillDictionary;

        /// <summary>
        ///     입력을 처리하는 클래스입니다.
        /// </summary>
        public CommandSystem(ICreatureServiceProvider serviceProvider, CharacterType type, IReadOnlyList<CommandAction> skillPresets)
        {
            _serviceProvider = serviceProvider;
            _skillDictionary = new Dictionary<BlockType, CommandAction>();

            RegisterCommandAction(skillPresets);
        }
        
        public CommandAction GetCommandAction(BlockType blockType)
        {
            return _skillDictionary.GetValueOrDefault(blockType);
        }

        private void RegisterCommandAction(IReadOnlyList<CommandAction> skillPresets)
        {
            for (var i = 0; i < Enum.GetValues(typeof(BlockType)).Length - 1; i++)
            {
                _skillDictionary.Add((BlockType)i, skillPresets[i]);
                
                Debug.Log($"{(BlockType)i} 커맨드 추가 / Value : {skillPresets[i].GetType()}");
            }
        }
        
        /// <summary>
        ///     입력을 처리합니다.
        /// </summary>
        public void ActivateCommand(BlockType blockType, int count)
        {
            _skillDictionary[blockType].ActivateCommand(count);
        }
        
        public void Clear()
        {
            _skillDictionary.Clear();
        }
    }
}