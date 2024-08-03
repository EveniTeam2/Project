using System;

namespace Unit.GameScene.Units.Panels.Interfaces
{
    public interface IIncreaseDragCount
    {
        event Action<int> OnIncreaseDragCount;
    }
}