using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.Data;
using ScriptableObjects.Scripts.Creature.Data.KnightData;
using ScriptableObjects.Scripts.Creature.Data.WizardData;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.SkillFactories.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;

namespace Unit.GameScene.Units.SkillFactories.Units.CharacterSkillFactories
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

        public override Dictionary<string, CharacterSkill> CreateSkill(List<SkillData> skillCsvData)
        {
            return _characterClassType switch
            {
                CharacterClassType.Knight => new KnightSkillFactory((KnightDataSo)_characterDataSo).CreateSkill(skillCsvData),
                CharacterClassType.Wizard => new WitchSkillFactory((WizardDataSo)_characterDataSo).CreateSkill(skillCsvData),
                CharacterClassType.Centaurs => new CentaursSkillFactory(null).CreateSkill(skillCsvData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}