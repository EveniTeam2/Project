using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.Settings;
using ScriptableObjects.Scripts.Creature.Settings.KnightDefaultSetting;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules.Unit.Character;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    public class CharacterSetting
    {
        public Creature Prefab { get; }
        public CharacterType Type { get; }
        public CharacterStat Stat { get; }
        
        public List<string> CharacterSkillPresets { get; }
        public Dictionary<string, Sprite> CharacterSkillIcons { get; }
        public Dictionary<string, int> CharacterSkillIndexes { get; }
        public Dictionary<string, float> CharacterSkillValues { get; }

        public CharacterSetting(CharacterDefaultSetting characterDefaultSetting, CharacterExtraSetting characterExtraSetting)
        {
            Prefab = characterDefaultSetting.baseCreature;
            Type = characterDefaultSetting.characterType;
            Stat = characterDefaultSetting.characterStat;
            CharacterSkillPresets = characterExtraSetting.characterSkillPresets;
            
            CharacterSkillIcons = new Dictionary<string, Sprite>();
            CharacterSkillIndexes = new Dictionary<string, int>();
            CharacterSkillValues = new Dictionary<string, float>();
            
            switch (characterDefaultSetting.characterType)
            {
                case CharacterType.Knight:
                    var knightDefaultSetting = (KnightDefaultSetting)characterDefaultSetting;

                    foreach (var knightSkillSetting in knightDefaultSetting.knightSkillSettings)
                    {
                        CharacterSkillIcons.Add($"{knightSkillSetting.skillType}", knightSkillSetting.skillIcon);
                        CharacterSkillIndexes.Add($"{knightSkillSetting.skillType}", knightSkillSetting.skillIndex);
                        CharacterSkillValues.Add($"{knightSkillSetting.skillType}", knightSkillSetting.skillValue);
                    }

                    break;
                case CharacterType.Wizard:
                    break;
                case CharacterType.Centaurs:
                    break;
            }
        }
    }
}