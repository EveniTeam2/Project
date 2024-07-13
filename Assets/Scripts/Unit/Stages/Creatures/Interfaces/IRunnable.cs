using System;

namespace Unit.Stages.Creatures.Interfaces
{
    public interface IRunnable
    {
        event Action<IRunnable> OnRun;
        int Speed { get; }
        void SetRun(bool isRun);
        bool IsRun { get; }
    }
}