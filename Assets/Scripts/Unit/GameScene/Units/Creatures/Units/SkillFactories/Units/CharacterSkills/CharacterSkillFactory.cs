using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScriptableObjects.Scripts.Creature.Data;
using ScriptableObjects.Scripts.Creature.Data.KnightData;
using ScriptableObjects.Scripts.Creature.Data.WizardData;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Units.CharacterSkills.Units;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories.Units.CharacterSkills
{
    public class CharacterSkillFactory : SkillFactory
    {
        private readonly CharacterClassType _characterClassType;
        private readonly CharacterDataSo _characterDataSo;

        public CharacterSkillFactory(CharacterDataSo characterDataSo)
        {
            _characterDataSo = characterDataSo;
            _characterClassType = characterDataSo.classType;
        }

        public override Dictionary<string, CharacterSkill> CreateSkill()
        {
            return _characterClassType switch
            {
                CharacterClassType.Knight => new KnightSkillFactory((KnightDataSo)_characterDataSo).CreateSkill(),
                CharacterClassType.Wizard => new WitchSkillFactory((WizardDataSo)_characterDataSo).CreateSkill(),
                // CharacterClassType.Centaurs => expr,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}