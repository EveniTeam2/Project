using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.Data.WizardData;
using Unit.GameScene.Units.SkillFactories.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;

namespace Unit.GameScene.Units.SkillFactories.Units.CharacterSkillFactories
{
    public class WitchSkillFactory : SkillFactory
    {
        private WizardDataSo _dataSo;
        
        public WitchSkillFactory(WizardDataSo dataSo)
        {
            _dataSo = dataSo;
        }

        public override Dictionary<string, CharacterSkill> CreateSkill(List<SkillData> skillCsvData)
        {
            throw new NotImplementedException();
        }
    }
}