using System;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Wizard.Enums;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Wizard.Modules
{
    [Serializable]
    public class WizardSkillData : CharacterSkillData
    {
        public WizardSkillType skillName;
    }
}