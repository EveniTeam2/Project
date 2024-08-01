using System;
using Unit.GameScene.Units.Creatures.Interfaces.Stats;
using Unit.GameScene.Units.Creatures.Module;

namespace Unit.GameScene.Units.Creatures.Units.Monsters
{
    public enum eEventType
    {
        AnimationEnd,
    }
    public class MonsterEventPublisher
    {
        public event Action<MonsterBattleStat> _onAnimationAttackEvent;
        public event Action _onAnimationEndEvent;

        internal void CallAttackEvent(MonsterBattleStat stat)
        {
            _onAnimationAttackEvent?.Invoke(stat);
        }

        internal void CallAnimationEndEvent()
        {
            _onAnimationEndEvent?.Invoke();
        }

        public void RegistOnAttackEvent(Action<MonsterBattleStat> subscirbe)
        {
            _onAnimationAttackEvent += subscirbe;
        }

        public void UnregistOnAttackEvent(Action<MonsterBattleStat> subscirbe)
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