namespace Unit.GameScene.Units.Creatures.Interfaces
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