namespace Unit.GameScene.Units.Creatures.Interfaces
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