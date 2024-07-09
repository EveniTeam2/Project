using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Unit.Character {
    public interface IDamageable {
        public event Action<IDamageable> OnDamage;
        public int Health { get; }
        public bool IsDead { get; }
        event Action<BaseCharacter> OnDeath;
        void Damage(int dmg);
        void SetHealth(int health);
    }

    public interface IRunnable {
        public event Action<IRunnable> OnRun;
        public int Speed { get; }
        public void SetRun(bool isRun);
        public bool IsRun { get; }
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
        public LinkedList<int> spdModifier = new LinkedList<int>();
        public abstract int Speed { get; }
        protected static Vector3 _zeroPosition;
        protected static int spdUnit = 1000;
        public bool IsRun { get; protected set; }
        public StateMachine HFSM { get; protected set; }
        public List<string> AnimationParameter;
        public event Action<IRunnable> OnRun;

        public abstract float GetCurrentPosition();
        public abstract void SetRun(bool isRun);
        protected abstract void RecalculateSpeed();

        public int ModifySpeed(int spd, float duration) {
            if (duration > 0) {
                StartCoroutine(ModifierSpeed(spd, duration));
            }
            else {
                spdModifier.AddLast(spd);
                RecalculateSpeed();
            }
            return Speed;
        }
        protected IEnumerator ModifierSpeed(int spd, float duration) {
            spdModifier.AddLast(spd);
            RecalculateSpeed();
            yield return new WaitForSeconds(duration);
            spdModifier.Remove(spd);
            RecalculateSpeed();
        }
        protected virtual void Update() {
            if (IsRun) {
                OnRun?.Invoke(this);
            }
        }
    }
}