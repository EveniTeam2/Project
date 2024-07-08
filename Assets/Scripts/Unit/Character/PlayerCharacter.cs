using System;

namespace Unit.Character {
    public class PlayerCharacter : BaseCharacter, IDamageable {
        public int Health => _health.Current;
        public override int Speed => _speed.Current;
        public event Action<BaseCharacter> OnDeath;
        public InstanceStat<int> _health;
        public InstanceStat<int> _speed;
        private bool _run;
        public void Damage(int dmg) {
            _health.Current -= dmg;
            if (_health.Current <= 0) {
                _health.Current = 0;
                OnDeath?.Invoke(this);
            }
        }

        public override float GetCurrentPosition() {
            return 0;
        }

        public void SetHealth(int health) {
            _health = new InstanceStat<int>(health);
        }

        public override void SetRun(bool isRun) {
            _run = isRun;
        }

        protected override void RecalculateSpeed() {
            if (_run) {
                _speed.Current = _speed.Origin;
                foreach (var spd in spdModifier) {
                    _speed.Current += spd;
                }
            }
        }

        private void Awake() {
            _zeroPosition = transform.position;
        }

        private void Update() {
            if (_run) {

            }
            else {

            }
        }

        internal void Initialize(PlayerStat stat) {
            _speed.Origin = stat.Speed;
            _health.Origin = stat.Health;
        }
    }
}