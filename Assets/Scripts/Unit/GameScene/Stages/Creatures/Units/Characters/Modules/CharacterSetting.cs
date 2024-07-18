using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.Settings;
using ScriptableObjects.Scripts.Creature.Settings.KnightDefaultSetting;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules.Unit.Character;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Units.CharacterSkills;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Modules
{
    public class CharacterSetting
    {
        public Creature Prefab { get; }
        public CharacterType Type { get; }
        public CharacterStat Stat { get; }
        public List<CharacterSkill> CharacterSkills { get; }
        public Dictionary<string, int> CharacterSkillIndexs { get; }

        public CharacterSetting(CharacterDefaultSetting characterDefaultSetting, CharacterExtraSetting characterExtraSetting)
        {
            Prefab = characterDefaultSetting.baseCreature;
            Type = characterDefaultSetting.characterType;
            Stat = characterDefaultSetting.characterStat;

            CharacterSkillIndexs = new Dictionary<string, int>();
            
            switch (characterDefaultSetting.characterType)
            {
                case CharacterType.Knight:
                    var knightDefaultSetting = (KnightDefaultSetting)characterDefaultSetting;

                    foreach (var knightSkillType in knightDefaultSetting.knightSkillTypes)
                    {
                        CharacterSkillIndexs.Add($"{knightSkillType.skillType}", knightSkillType.skillIndex);
                    }
                    
                    break;
                case CharacterType.Wizard:
                    break;
                case CharacterType.Centaurs:
                    break;
            }

            

            CharacterSkills = new CharacterSkillFactory(Type, characterExtraSetting.characterSkillPresets).CreateSkill();
        }
    }
}