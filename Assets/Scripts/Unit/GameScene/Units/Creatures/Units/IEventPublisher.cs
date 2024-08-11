namespace Unit.GameScene.Units.Creatures.Units
{
    public interface IEventPublisher
    {
        public void RegisterEvent(IEventSubscriber eventSubscriber);
        public void UnregisterEvent(IEventSubscriber eventSubscriber);
    }
}