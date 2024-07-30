using System;
using Unit.GameScene.Module;

namespace Unit.GameScene.Units.Panels.BoardPanels.Units.MatchBlockPanels.Interfaces
{
    public interface ISendCommand
    {
        event Action<CommandPacket> OnSendCommand;
    }
}