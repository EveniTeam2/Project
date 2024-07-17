using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.Settings;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules.Unit.Character;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Units.CharacterSkills;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Modules
{
    public class CharacterSetting
    {
        [Header("캐릭터 프리팹"), Space(5)] public readonly Creature Prefab;
        [Header("캐릭터 기본 타입"), Space(5)] public readonly CharacterType Type;
        [Header("캐릭터 기본 스탯"), Space(5)] public CharacterStat Stat;
        [Header("캐릭터 스킬 프리셋"), Space(5)] public readonly List<CharacterSkill> CharacterSkills;
        
        public CharacterSetting(CharacterDefaultSetting characterDefaultSetting, CharacterExtraSetting characterExtraSetting)
        {
            Prefab = characterDefaultSetting.baseCreature;
            Type = characterDefaultSetting.characterType;
            Stat = characterDefaultSetting.characterStat;

            CharacterSkills = new CharacterSkillFactory(Type, characterExtraSetting.characterSkillPresets).CreateSkill();
        }

        public void RegisterCharacterReference(Character character)
        {
            foreach (var characterSkill in CharacterSkills)
            {
                characterSkill.RegisterCharacterReference(character);
            }
        }
    }
}