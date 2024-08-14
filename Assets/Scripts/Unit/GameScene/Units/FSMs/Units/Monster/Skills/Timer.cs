using Unit.GameScene.Units.Creatures.Units;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    public class Timer : IEventSubscriber
    {
        public bool IsDone { get; protected set; }
        private float targetTime;
        private float passedTime;
        private IEventPublisher targetPublisher;

        public Timer(IEventPublisher targetPublisher, float targetTime)
        {
            this.targetPublisher = targetPublisher;
            this.targetTime = targetTime;
        }

        void IEventSubscriber.OnEvent()
        {
            passedTime += Time.deltaTime;
            if (passedTime > targetTime)
            {
                Reset();
            }
        }

        public void Reset()
        {
            IsDone = true;
            passedTime = 0;
            targetPublisher.UnregisterEvent(this);
        }

        public void Start()
        {
            Reset();
            IsDone = false;
            targetPublisher.RegisterEvent(this);
        }
    }
}