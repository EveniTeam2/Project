using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.Data;
using ScriptableObjects.Scripts.Creature.Data.KnightData;
using ScriptableObjects.Scripts.Creature.Data.WizardData;
using System.Linq;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.SkillFactories.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;

namespace Unit.GameScene.Units.SkillFactories.Units.CharacterSkillFactories
{
    public class CharacterSkillFactory : SkillFactory
    {
        private readonly CharacterType _characterType;
        private readonly CharacterDataSo _characterDataSo;

        public CharacterSkillFactory(CharacterDataSo characterDataSo)
        {
            _characterDataSo = characterDataSo;
            _characterType = characterDataSo.type;
        }

        public override Dictionary<string, CharacterSkill> CreateSkill(List<SkillData> skillCsvData)
        {
            var csvData = skillCsvData.Where(data => data.CharacterType == _characterType).ToList();
            
;           return _characterType switch
            {
                CharacterType.Knight => new KnightSkillFactory((KnightDataSo)_characterDataSo).CreateSkill(csvData),
                CharacterType.Wizard => new WitchSkillFactory((WizardDataSo)_characterDataSo).CreateSkill(csvData),
                CharacterType.Centaurs => new CentaursSkillFactory(null).CreateSkill(csvData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}