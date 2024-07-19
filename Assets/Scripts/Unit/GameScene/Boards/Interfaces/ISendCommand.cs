using System;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Manager.Modules;

namespace Unit.GameScene.Boards.Interfaces
{
    public interface ISendCommand
    {
        event Action<CommandPacket> OnSendCommand;
    }
}