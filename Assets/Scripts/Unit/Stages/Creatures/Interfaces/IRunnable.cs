using System;

namespace Unit.Stages.Creatures.Interfaces {
    public interface IRunnable
    {
        int Speed { get; }
        void SetRun(bool isRun);
        bool IsRun { get; }
        void Update();
        void FixedUpdate();
    }
}