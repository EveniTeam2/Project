using System;

namespace Unit.GameScene.Units.Creatures.Units
{
    public class ManualUpdater : IEventPublisher
    {
        private event Action UpdateEvent;
        public void Update()
        {
            UpdateEvent?.Invoke();
        }
        public void RegisterEvent(IEventSubscriber eventSubscriber)
        {
            UpdateEvent += eventSubscriber.OnEvent;
        }
        public void UnregisterEvent(IEventSubscriber eventSubscriber)
        {
            UpdateEvent -= eventSubscriber.OnEvent;
        }
    }
}