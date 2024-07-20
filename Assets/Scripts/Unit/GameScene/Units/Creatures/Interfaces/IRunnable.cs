namespace Unit.GameScene.Stages.Creatures.Interfaces
{
    public interface IRunnable
    {
        int Speed { get; }
        bool IsRun { get; }
        void SetRun(bool isRun);
        void Update();
        void FixedUpdate();
    }
}