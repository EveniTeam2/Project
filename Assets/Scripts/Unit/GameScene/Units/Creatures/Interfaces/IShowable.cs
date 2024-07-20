namespace Unit.GameScene.Stages.Creatures.Interfaces
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