using UnityEngine;
using Unit.Stages.Creatures.Characters;
using Unit.Stages.Creatures.Characters.Unit.Character;
using Unit.Stages.Creatures.Monsters;
using System;

namespace Unit.Stages.Creatures {
    public class BattleSystem {
        protected BaseCreature _character;
        protected BattleStat _stat;
        public BattleSystem(PlayerCreature self, Stat<CharacterStat> stat) {
            _character = self;
            _stat = new BattleStat(stat);

        }
        public BattleSystem(MonsterCreature target, Stat<MonsterStat> stat) {
            _character = target;
            _stat = new BattleStat(stat);
        }
        public bool CheckCollider(LayerMask targetLayer, float distance, out RaycastHit2D[] collidee) {
            var hits = Physics2D.RaycastAll(_character.transform.position, new Vector2(1, 0), distance, targetLayer);
            collidee = hits;
            return false;
        }

        internal void Attack(RaycastHit2D col) {
            // TODO col.collider.GetInstanceID()를 다 들고 이를 통해 BaseCreature로 접근할 수 있게 미리 캐싱해두어 Dictionary<string, BaseCreature> spawnCreature가 있으면 좋겠습니다.
            throw new NotImplementedException();
        }
    }

    public class BattleStat {
        public int Attack => GetAttack.Invoke();
        public int Health => GetHealth.Invoke();
        Func<int> GetAttack;
        Func<int> GetHealth;
        public BattleStat(Stat<CharacterStat> stat) {
            GetAttack = () => stat.Current.Attack;
            GetHealth = () => stat.Current.Health;
        }
        public BattleStat(Stat<MonsterStat> stat) {
            GetAttack = () => stat.Current.Attack;
            GetHealth = () => stat.Current.Health;
        }
    }
}