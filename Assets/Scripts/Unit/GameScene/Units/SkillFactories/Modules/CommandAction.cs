using Unit.GameScene.Units.SkillFactories.Interfaces;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;

namespace Unit.GameScene.Units.SkillFactories.Modules
{
    public class CommandAction
    {
        private ISkillCommandAction _skillCommandAction;

        public void Initialize(ISkillCommandAction skillCommandAction)
        {
            _skillCommandAction = skillCommandAction;
        }

        public void Clear()
        {
            _skillCommandAction = null;
        }

        public void Execute(int comboCount)
        {
            _skillCommandAction.Execute(comboCount);
        }
        
        public void ActivateCommandAction()
        {
            if (_skillCommandAction is CharacterSkill characterSkill)
            {
                characterSkill.ActivateSkillEffects();
            }
        }
    }
}