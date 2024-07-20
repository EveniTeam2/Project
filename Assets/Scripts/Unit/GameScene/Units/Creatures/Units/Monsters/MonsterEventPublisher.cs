using System;
using Unit.GameScene.Stages.Creatures.Module;

namespace Unit.GameScene.Stages.Creatures.Units.Monsters
{
    public enum eEventType
    {
        AnimationEnd,
    }
    public class MonsterEventPublisher
    {
        public event Action<IBattleStat> _onAnimationAttackEvent;
        public event Action _onAnimationEndEvent;

        internal void CallAttackEvent(IBattleStat stat)
        {
            _onAnimationAttackEvent?.Invoke(stat);
        }

        internal void CallAnimationEndEvent()
        {
            _onAnimationEndEvent?.Invoke();
        }

        public void RegistOnAttackEvent(Action<IBattleStat> subscirbe)
        {
            _onAnimationAttackEvent += subscirbe;
        }

        public void UnregistOnAttackEvent(Action<IBattleStat> subscirbe)
        {
            _onAnimationAttackEvent -= subscirbe;
        }

        public void RegistOnEvent(eEventType eventType, Action subscirbe)
        {
            switch (eventType)
            {
                case eEventType.AnimationEnd:
                    _onAnimationEndEvent += subscirbe;
                    break;
            }
        }

        public void UnregistOnEvent(eEventType eventType, Action subscirbe)
        {
            switch (eventType)
            {
                case eEventType.AnimationEnd:
                    _onAnimationEndEvent -= subscirbe;
                    break;
            }
        }
    }
}