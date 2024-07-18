using System;
using Unit.GameScene.Stages.Creatures.Units.Monsters;

namespace Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput {
    public class MonsterHealthSystem : HealthSystem {
        public MonsterHealthSystem(MonsterHealthStat stats) : base(stats) {
        }
        public override void Damage(int dmg) {
            // TODO 방어력 있으면 적용해야되는 곳
            _health = dmg;
            if (_health <= 0) {
                _health = 0;
                CallDeath();
            }
            CallDamage();
        }
    }
    public class MonsterHealthStat : IHealthStat {
        private Func<int> _getHealth;
        private Action<int> _setHealth;
        public MonsterHealthStat(Stat<MonsterStat> stat) {
            _getHealth = () => stat.Current.Health;
            _setHealth = (value) => stat.Current.Health = value;
        }

        public int GetHealth() {
            return _getHealth.Invoke();
        }

        public void SetHealth(int health) {
            _setHealth?.Invoke(health);
        }
    }
}