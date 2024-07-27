using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.Data.WizardData;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories.Units.CharacterSkills.Units
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