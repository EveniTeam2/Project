using System;

namespace Unit.GameScene.Units.BoardPanels.Units.MatchBlockPanels.Interfaces
{
    public interface IIncreaseDragCount
    {
        event Action<int> OnIncreaseDragCount;
    }
}