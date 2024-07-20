using System;

namespace Unit.GameScene.Boards.Interfaces
{
    public interface IIncreaseDragCount
    {
        event Action<int> OnIncreaseDragCount;
    }
}