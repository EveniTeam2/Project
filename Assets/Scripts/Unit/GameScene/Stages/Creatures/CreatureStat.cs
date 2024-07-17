namespace Unit.GameScene.Stages.Creatures
{
    public class Stat<T>
    {
        public Stat(T origin)
        {
            Origin = origin;
            Current = origin;
        }

        public T Origin { get; private set; }
        public T Current { get; private set; }

        public void SetOrigin(T origin)
        {
            Origin = origin;
        }

        public void SetCurrent(T current)
        {
            Current = current;
        }
    }
}