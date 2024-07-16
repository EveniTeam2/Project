using System;
using Unit.GameScene.Manager.Interfaces;

namespace Unit.GameScene.Boards.Interfaces
{
    public interface ISendCommand
    {
        event Action<ICommand<IStage>> OnSendCommand;
    }
}