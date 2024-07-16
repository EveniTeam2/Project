namespace Unit.GameScene.Stages.Creautres.Interfaces
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