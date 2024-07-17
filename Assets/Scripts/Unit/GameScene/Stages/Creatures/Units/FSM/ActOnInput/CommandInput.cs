using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unit.GameScene.Boards.Blocks.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using Debug = UnityEngine.Debug;

namespace Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput
{
    /// <summary>
    ///     사용자 입력을 처리하는 클래스입니다.
    /// </summary>
    public class CommandInput
    {
        private readonly Character _character;
        private readonly Dictionary<BlockType, Skill> _skillDictionary;

        /// <summary>
        ///     입력을 처리하는 클래스입니다.
        /// </summary>
        public CommandInput(Character target, CharacterType type, IReadOnlyList<Skill> skillPresets)
        {
            _character = target;
            _skillDictionary = new Dictionary<BlockType, Skill>();

            switch (type)
            {
                case CharacterType.Knight:
                    for (var i = 0; i < Enum.GetValues(typeof(BlockType)).Length; i++)
                    {
                        _skillDictionary.Add((BlockType) i, skillPresets[i]);
                    }
                    break;
                case CharacterType.Wizard:
                    break;
                case CharacterType.Centaurs:
                    break;
            }
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

        public void Clear()
        {
            _skillDictionary.Clear();
        }

        /// <summary>
        ///     입력을 처리합니다.
        /// </summary>
        public void Input(BlockType blockType, int count)
        {
            // TODO : After
            // _skillDictionary[blockType].ActivateSkill(_character, count);
        }
    }
}