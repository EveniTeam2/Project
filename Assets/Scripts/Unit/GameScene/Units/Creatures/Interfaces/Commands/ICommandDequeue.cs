using System;

namespace Unit.GameScene.Units.Creatures.Interfaces.Commands
{
    public interface ICommandDequeue
    {
        public event Action OnCommandDequeue;
    }
}