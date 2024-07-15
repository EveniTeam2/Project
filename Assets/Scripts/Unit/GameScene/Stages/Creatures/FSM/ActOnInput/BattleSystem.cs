using System;
using Unit.GameScene.Stages.Creatures.Characters;
using Unit.GameScene.Stages.Creatures.Characters.Unit.Character;
using Unit.GameScene.Stages.Creatures.Monsters;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.FSM.ActOnInput {
    public class BattleSystem {
        protected BaseCreature _character;
        protected StageManager _stageManager;
        protected BattleStat _stat;
        public BattleSystem(StageManager manager, PlayerCreature self, Stat<CharacterStat> stat) {
            _character = self;
            _stat = new BattleStat(stat);
            _stageManager = manager;
        }
        public BattleSystem(StageManager manager, MonsterCreature target, Stat<MonsterStat> stat) {
            _character = target;
            _stat = new BattleStat(stat);
            _stageManager = manager;
        }
        public bool CheckCollider(LayerMask targetLayer, Vector2 direction, float distance, out RaycastHit2D[] collidee) {
            var hits = Physics2D.RaycastAll(_character.transform.position, direction, distance, targetLayer);
            collidee = hits;
            if (collidee.Length > 0)
                return true;
            else
                return false;
        }
        internal void Attack(RaycastHit2D col) {
            // TODO col.collider.GetInstanceID()를 다 들고 이를 통해 BaseCreature로 접근할 수 있게 미리 캐싱해두어 Dictionary<string, BaseCreature> spawnCreature가 있으면 좋겠습니다.
            foreach (var mon in _stageManager.Monsters) {
                if (mon.gameObject.GetInstanceID() == col.collider.gameObject.GetInstanceID()) {
                    mon.Health.Damage(_stat.Attack);
                }
            }
        }
    }

    public class BattleStat {
        public int Attack => GetAttack.Invoke();
        Func<int> GetAttack;
        public BattleStat(Stat<CharacterStat> stat) {
            GetAttack = () => stat.Current.Attack;
        }
        public BattleStat(Stat<MonsterStat> stat) {
            GetAttack = () => stat.Current.Attack;
        }
    }
}