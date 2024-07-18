using System;
using System.Collections.Generic;
using Unit.GameScene.Boards.Blocks.Enums;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Module
{
    /// <summary>
    ///     사용자 입력을 처리하는 클래스입니다.
    /// </summary>
    public class CommandInput
    {
        private readonly ICreatureServiceProvider _serviceProvider;
        private readonly Dictionary<BlockType, CharacterSkill> _skillDictionary;

        /// <summary>
        ///     입력을 처리하는 클래스입니다.
        /// </summary>
        public CommandInput(ICreatureServiceProvider serviceProvider, CharacterType type,
            IReadOnlyList<CharacterSkill> skillPresets)
        {
            _serviceProvider = serviceProvider;
            _skillDictionary = new Dictionary<BlockType, CharacterSkill>();

            RegisterSkillToCommand(skillPresets);
        }

        private void RegisterSkillToCommand(IReadOnlyList<CharacterSkill> skillPresets)
        {
            for (var i = 0; i < Enum.GetValues(typeof(BlockType)).Length - 1; i++)
            {
                skillPresets[i].RegisterServiceProvider(_serviceProvider);
                _skillDictionary.Add((BlockType)i, skillPresets[i]);
                
                Debug.Log($"{(BlockType)i} 커맨드 추가 / Value : {skillPresets[i].GetType()}");
            }
        }
        
        /// <summary>
        ///     입력을 처리합니다.
        /// </summary>
        public void Input(BlockType blockType, int count)
        {
            _skillDictionary[blockType].ActivateSkill(count);
        }
        
        public void Clear()
        {
            _skillDictionary.Clear();
        }

//         private bool AddInput(ActOnInput act)
//         {
//             // return skillDictionary.TryAdd(act.BlockType, act);
//         }
//
//         public void AddInput(ActOnInput[] acts)
//         {
//             foreach (var act in acts)
//             {
// #if UNITY_EDITOR
//                 if (!AddInput(act)) Debug.LogWarning(_character.name + " already has same input.");
// #else
//                 AddInput(act);
// #endif
//             }
//         }
    }
}