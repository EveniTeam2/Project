using Unit.Stages.Creatures.Characters;
using Unit.Stages.Creatures.Characters.Unit.Character;
using Unit.Stages.Creatures.Monsters;
using System;
using Unit.Stages.Creatures.Interfaces;

namespace Unit.Stages.Creatures {
    public class HealthSystem : IDamageable {
        int IDamageable.Health => _health;
        bool IDamageable.IsDead => _isDead;
        event Action<BaseCreature> IDamageable.OnDeath { add { _onDeath += value; } remove { _onDeath -= value; } }

        private bool _isDead => _health <= 0;
        private int _health => GetHeatlh.Invoke();
        private event Action<BaseCreature> _onDeath;
        private Func<int> GetHeatlh;
        private Action<int> SetHealth;

        private BaseCreature _character;
        public HealthSystem(PlayerCreature character, Stat<CharacterStat> stats) {
            _character = character;
            GetHeatlh = () => stats.Current.Health;
            SetHealth = (value) => {
                var cur = stats.Current;
                cur.Health -= value;
                stats.SetCurrent(cur);
            };
        }
        public HealthSystem(MonsterCreature monster, Stat<MonsterStat> stats) {
            _character = monster;
            GetHeatlh = () => stats.Current.Health;
        }
        void IDamageable.Damage(int dmg) {
            SetHealth.Invoke(_health - dmg);
            if (_health < 0) {
                SetHealth.Invoke(0);
                _onDeath?.Invoke(_character);
            }
        }
    }
}