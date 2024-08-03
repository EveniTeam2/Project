using System;
using Unit.GameScene.Module;

namespace Unit.GameScene.Units.Panels.Interfaces
{
    public interface ISendCommand
    {
        event Action<CommandPacket> OnSendCommand;
    }
}