using Unit.GameScene.Units.Creatures.Module.SkillFactories.Interfaces;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills.Abstract;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules
{
    public class CommandAction
    {
        private ISkillCommand _skillCommand;

        public void Initialize(ISkillCommand skillCommand)
        {
            _skillCommand = skillCommand;
        }

        public void Clear()
        {
            _skillCommand = null;
        }

        public void Execute(int comboCount)
        {
            _skillCommand.Execute(comboCount);
        }
        
        public void ActivateCommandAction()
        {
            if (_skillCommand is CharacterSkill characterSkill)
            {
                characterSkill.ActivateSkillEffects();
            }
        }
    }
}