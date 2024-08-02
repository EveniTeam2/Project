using System;

namespace Unit.GameScene.Units.Panels.BoardPanels.Units.MatchBlockPanels.Interfaces
{
    public interface IIncreaseDragCount
    {
        event Action<int> OnIncreaseDragCount;
    }
}