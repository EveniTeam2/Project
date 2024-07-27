using System;
using System.Collections;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures;
using UnityEngine;

namespace Assets.Scripts.Unit.GameScene.Units.Creatures.Units
{
    public interface IBattleEffect
    {
        void Attack<TargetType>(TargetType target) where TargetType : Creature;
    }

    public class FireBattleEffect : IBattleEffect
    {
        private float duration;
        private float delay;
        private int damagePerSeconds;

        public FireBattleEffect(float duration, float delay, int damagePerSeconds)
        {
            this.duration = duration;
            this.delay = delay;
            this.damagePerSeconds = damagePerSeconds;
        }

        public void Attack<Target>(Target creature) where Target : Creature
        {
            creature.StartCoroutine(FireEffect(creature, duration, delay, damagePerSeconds));
        }

        private IEnumerator FireEffect<Target>(Target creature, float duration, float delay, int damagePerSeconds)
        {
            float passedTime = 0f;
            while (passedTime < duration)
            {
                // TODO : apply effect
                yield return null;
            }
        }
    }
}
