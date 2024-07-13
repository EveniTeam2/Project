namespace Unit.Stages.Creatures
{
    public class Stat<T> where T : struct
    {
        public T Origin { get; private set; }
        public T Current { get; private set; }

        public Stat(T origin)
        {
            Origin = origin;
            Current = origin;
        }

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