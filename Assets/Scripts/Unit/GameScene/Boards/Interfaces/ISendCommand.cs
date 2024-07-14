using System;
using Unit.GameScene.Stages.Interfaces;

namespace Unit.GameScene.Boards.Interfaces
{
    public interface ISendCommand
    {
        event Action<ICommand<IStageCreature>> OnSendCommand;
    }
}