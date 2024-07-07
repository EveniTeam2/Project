using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.Character {
    public interface IDamageable {
        public int Health { get; }
        event Action<BaseCharacter> OnDeath;
        void Damage(int dmg);
        void SetHealth(int health);
    }

    public interface IRunnable {
        public float Speed { get; }
        public void SetSpeed(float spd);
    }

    public class InstanceStat<T> where T : struct {
        public T Origin;
        public T Current;
        public InstanceStat(T origin) {
            Origin = origin;
            Current = origin;
        }
    }

    public abstract class BaseCharacter : MonoBehaviour, IRunnable {
        public abstract float Speed { get; }

        public abstract float GetCurrentPosition();
        public abstract void SetSpeed(float spd);
    }
}