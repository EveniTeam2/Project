using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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
        public Creature Prefab { get; }
        public CharacterType Type { get; }
        public CharacterStat Stat { get; }
        public List<CharacterSkill> CharacterSkills { get; }
        public Dictionary<AnimationParameterEnums, int> CharacterAnimationParameter { get; private set; }

        private readonly AnimationParameterEnums[] _animationParameterEnums;
        
        public CharacterSetting(CharacterDefaultSetting characterDefaultSetting, CharacterExtraSetting characterExtraSetting)
        {
            Prefab = characterDefaultSetting.baseCreature;
            Type = characterDefaultSetting.characterType;
            Stat = characterDefaultSetting.characterStat;
            _animationParameterEnums = characterDefaultSetting.creatureAnimationParameter;
            
            CharacterSkills = new CharacterSkillFactory(Type, characterExtraSetting.characterSkillPresets).CreateSkill();

            ChangeAnimationParameterToHash();
        }

        public void RegisterCharacterReference(Character character)
        {
            foreach (var characterSkill in CharacterSkills)
            {
                characterSkill.RegisterCharacterReference(character);
            }
        }

        private void ChangeAnimationParameterToHash()
        {
            CharacterAnimationParameter = new Dictionary<AnimationParameterEnums, int>();
            
            foreach (var animationParameter in _animationParameterEnums)
            {
                CharacterAnimationParameter.Add(animationParameter, Animator.StringToHash($"{animationParameter}"));
            }
        }
    }
}