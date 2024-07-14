using System;

namespace Unit.GameScene.Stages.Interfaces
{
    public interface ISendCommand
    {
        event Action<ICommand<IStageCreature>> OnSendCommand;
    }
}