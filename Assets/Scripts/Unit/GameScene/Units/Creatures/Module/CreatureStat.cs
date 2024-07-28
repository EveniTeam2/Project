namespace Unit.GameScene.Units.Creatures.Module
{
    public class CreatureStat<T>
    {
        public CreatureStat(T origin)
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