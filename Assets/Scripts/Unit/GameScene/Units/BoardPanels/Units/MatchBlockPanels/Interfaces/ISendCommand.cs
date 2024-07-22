using System;
using Unit.GameScene.Manager.Modules;

namespace Unit.GameScene.Units.BoardPanels.Units.MatchBlockPanels.Interfaces
{
    public interface ISendCommand
    {
        event Action<CommandPacket> OnSendCommand;
    }
}