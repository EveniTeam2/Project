using Unit.GameScene.Units.Creatures.Module.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Interfaces;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules
{
    public class CommandAction
    {
        private readonly ISkillCommand _skillCommand;

        public CommandAction(ISkillCommand skillCommand)
        {
            _skillCommand = skillCommand;
        }

        public void ActivateCommand(int comboCount)
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