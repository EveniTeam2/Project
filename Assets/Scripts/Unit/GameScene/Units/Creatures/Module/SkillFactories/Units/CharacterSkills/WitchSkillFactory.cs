using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.Data.WizardData;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Abstract;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills
{
    public class WitchSkillFactory : SkillFactory
    {
        private WizardDataSo _dataSo;
        
        public WitchSkillFactory(WizardDataSo dataSo)
        {
            _dataSo = dataSo;
        }

        public override Dictionary<string, CharacterSkill> CreateSkill()
        {
            throw new NotImplementedException();
        }
    }
}