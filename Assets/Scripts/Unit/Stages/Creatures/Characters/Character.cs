using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using ScriptableObjects.Scripts.Creature;
using Unit.Stages.Creatures.Characters.Unit.Character;
using Unit.Stages.Creatures.FSM;
using Unit.Stages.Creatures.Interfaces;
using Unit.Stages.Creatures.Interfaces.Unit.Character;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.Stages.Creatures.Characters
{
    public class Character : BaseCreature, IRunnable
    {
        public event Action<IRunnable> OnRun;
        public LinkedList<int> SpdModifier = new LinkedList<int>();
        
        [SerializeField] private CreatureStateData creatureStateData;
        
        public bool IsRun { get; protected set; }
        public int Speed => _stats.Current.Speed;
        public int Attack => _stats.Current.Attack;
        public override int Health
        {
            get => _stats.Current.Health;
            protected set => _stats.SetCurrent(new CharacterStat { Attack = _stats.Current.Attack, Health = value, Speed = _stats.Current.Speed });
        }
        
        public override Animator Animator => _animator;
        private Animator _animator;
        private Stat<CharacterStat> _stats;
        private UserInput _input;
        
        public void Initialize(CharacterStat stat, IShowable background)
        {
            _stats = new Stat<CharacterStat>(stat);
            OnRun += background.Move;
            HFSM = StateBuilder.BuildState(this, creatureStateData);
            _input = new UserInput(this);
            _animator = GetComponent<Animator>();
        }

        public void Input(NewBlock block, int count)
        {
            _input.Input(block, count);
        }

        public float GetCurrentPosition()
        {
            return 0;
        }

        public void SetRun(bool isRun)
        {
            IsRun = isRun;
        }

        private void RecalculateSpeed()
        {
            if (IsRun)
            {
                _stats.SetCurrent(new CharacterStat { Attack = _stats.Current.Attack, Health = _stats.Current.Health, Speed = _stats.Origin.Speed });
                foreach (var spd in SpdModifier)
                {
                    _stats.SetCurrent(new CharacterStat { Attack = _stats.Current.Attack, Health = _stats.Current.Health, Speed = _stats.Current.Speed + spd });
                }
            }
        }

        public int ModifySpeed(int spd, float duration)
        {
            if (duration > 0)
            {
                StartCoroutine(ModifierSpeed(spd, duration));
            }
            else
            {
                SpdModifier.AddLast(spd);
                RecalculateSpeed();
            }
            return Speed;
        }

        private IEnumerator ModifierSpeed(int spd, float duration)
        {
            SpdModifier.AddLast(spd);
            RecalculateSpeed();
            yield return new WaitForSeconds(duration);
            SpdModifier.Remove(spd);
            RecalculateSpeed();
        }

        private void Update()
        {
            if (IsRun)
            {
                OnRun?.Invoke(this);
            }
            HFSM?.Update(this);
        }

        public override void SetHealth(int health)
        {
            _stats.SetOrigin(new CharacterStat { Attack = _stats.Origin.Attack, Health = health, Speed = _stats.Origin.Speed });
            _stats.SetCurrent(new CharacterStat { Attack = _stats.Current.Attack, Health = health, Speed = _stats.Current.Speed });
        }

        private void FixedUpdate()
        {
            HFSM?.FixedUpdate(this);
        }
    }
}