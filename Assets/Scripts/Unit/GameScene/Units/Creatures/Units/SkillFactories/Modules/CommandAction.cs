using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories.Modules
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
    }
}