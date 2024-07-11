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

    public abstract class BaseCharacter : MonoBehaviour, IDamageable {
        public abstract int Health { get; protected set; }
        public StateMachine HFSM { get; protected set; }

        public bool IsDead => Health <= 0;
        
        public event Action<BaseCharacter> OnDeath;
        public event Action<IDamageable> OnDamage;
        
        public virtual void Damage(int dmg) {
            Health -= dmg;
            if (Health <= 0) {
                Health = 0;
                OnDeath?.Invoke(this);
            }
            OnDamage?.Invoke(this);
        }
        public abstract void SetHealth(int health);
        public abstract Animator Animator { get; }
    }
}