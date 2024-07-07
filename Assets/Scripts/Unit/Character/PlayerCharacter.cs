using System;

namespace Unit.Character {
    public class PlayerCharacter : BaseCharacter, IDamageable {
        public int Health => _health.Current;
        public override float Speed => _speed.Current;
        public event Action<BaseCharacter> OnDeath;
        public InstanceStat<int> _health;
        public InstanceStat<float> _speed;

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

        public override void SetSpeed(float spd) {
            _speed = new InstanceStat<float>(spd);
        }
    }
}