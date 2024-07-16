namespace Unit.GameScene.Stages.Creautres.Interfaces
{
    namespace Unit.Character
    {
        public interface IShowable
        {
            void Move(IRunnable target);
            void Stop();
        }
    }
}