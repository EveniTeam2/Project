using ScriptableObjects.Scripts.Blocks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Unit.Character {
    public class PlayerCharacter : BaseCharacter, IRunnable {
        InstanceStat<PlayerStat> _stats;
        public int Speed => _stats.Current.Speed;
        public int Attack => _stats.Current.Attack;
        [SerializeField] CharacterStateData characterStateData;
        UserInput _input;
        public event Action<IRunnable> OnRun;
        public LinkedList<int> spdModifier = new LinkedList<int>();
        public bool IsRun { get; protected set; }
        public override int Health { get => _stats.Current.Health; protected set => _stats.Current.Health = value; }
        public override Animator Animator => animator;
        [SerializeField] Animator animator;
        public virtual void Input(NewBlock block, int count) {
            _input.Input(block, count);
            // Idle run walk hit
            // 체력 회복 블록, 스킬 1번 실행 블론, 스킬 2번 ~ 4번
        }
        public virtual float GetCurrentPosition() {
            return 0;
        }
        public virtual void SetRun(bool isRun) {
            IsRun = isRun;
        }
        protected virtual void RecalculateSpeed() {
            if (IsRun) {
                _stats.Current.Speed = _stats.Origin.Speed;
                foreach (var spd in spdModifier) {
                    _stats.Current.Speed += spd;
                }
            }
        }
        public void Initialize(PlayerStat stat, IShowable background) {
            _stats = new InstanceStat<PlayerStat>(stat);
            OnRun += background.Move;
            HFSM = StateBuilder.BuildState(this, characterStateData);
            _input = new UserInput(this);
        }
        protected static int spdUnit = 1000;
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
            if (HFSM != null) HFSM.Update(this);
        }
        public override void SetHealth(int health) {
            _stats.Origin.Health = health;
            _stats.Current.Health = health;
        }
        private void FixedUpdate() {
            if (HFSM != null) HFSM.FixedUpdate(this);

        }
    }
}