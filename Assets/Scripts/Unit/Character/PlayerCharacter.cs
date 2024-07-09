using System;

namespace Unit.Character {
    public class PlayerCharacter : BaseCharacter, IDamageable {
        public int Health => _health.Current;
        public override int Speed => _speed.Current;
        public bool IsDead => _health.Current <= 0;
        public event Action<BaseCharacter> OnDeath;
        public event Action<IDamageable> OnDamage;
        public InstanceStat<int> _health;
        public InstanceStat<int> _speed;

        public void Damage(int dmg) {
            _health.Current -= dmg;
            if (_health.Current <= 0) {
                _health.Current = 0;
                OnDeath?.Invoke(this);
            }
            OnDamage?.Invoke(this);
        }

        public override float GetCurrentPosition() {
            return 0;
        }

        public void SetHealth(int health) {
            _health = new InstanceStat<int>(health);
        }

        public override void SetRun(bool isRun) {
            IsRun = isRun;
        }

        protected override void RecalculateSpeed() {
            if (IsRun) {
                _speed.Current = _speed.Origin;
                foreach (var spd in spdModifier) {
                    _speed.Current += spd;
                }
            }
        }

        private void Awake() {
            _zeroPosition = transform.position;
        }

        public void Initialize(PlayerStat stat, IShowable background) {
            _speed.Origin = stat.Speed;
            _health.Origin = stat.Health;
            // TODO HFSM에 IState 추가해야 한다.
            OnRun += background.Move;
        }
    }
}