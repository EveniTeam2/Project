using System;
using Unit.Stages.Creatures.FSM;
using Unit.Stages.Creatures.Interfaces;
using UnityEngine;

namespace Unit.Stages.Creatures
{
    public abstract class BaseCreature : MonoBehaviour, IDamageable
    {
        public abstract int Health { get; protected set; }
        public StateMachine HFSM { get; protected set; }

        public bool IsDead => Health <= 0;
    
        public event Action<BaseCreature> OnDeath;
        public event Action<IDamageable> OnDamage;
    
        /// <summary>
        /// 데미지를 입습니다.
        /// </summary>
        public virtual void Damage(int dmg)
        {
            Health -= dmg;
            if (Health <= 0)
            {
                Health = 0;
                OnDeath?.Invoke(this);
            }
            OnDamage?.Invoke(this);
        }

        /// <summary>
        /// 체력을 설정합니다.
        /// </summary>
        public abstract void SetHealth(int health);

        /// <summary>
        /// 애니메이터를 반환합니다.
        /// </summary>
        public abstract Animator Animator { get; }
    }
}