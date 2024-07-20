using System;
using System.Collections.Generic;
using Unit.GameScene.Boards.Blocks.Enums;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Modules;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Module
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

            RegisterSkillToCommand(skillPresets);
        }

        private void RegisterSkillToCommand(IReadOnlyList<CommandAction> skillPresets)
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