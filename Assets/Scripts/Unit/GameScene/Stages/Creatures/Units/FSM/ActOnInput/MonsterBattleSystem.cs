using System;
using Unit.GameScene.Stages.Creatures.Units.Characters;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput {
    public class MonsterBattleSystem : BattleSystem {
        public MonsterBattleSystem(Transform targetTransform, MonsterBattleStat stat) : base(targetTransform, stat) {
        }

        public override void Attack(RaycastHit2D col) {
            if (col.collider.gameObject.TryGetComponent<Character>(out var target)) {
#if UNITY_EDITOR
                var dmg = target.GetServiceProvider().Damage(_stat.GetAttack());
                Debug.Log($"몬스터가 {col.collider.gameObject.name}에게 {dmg} 피해를 입혔습니다.");
#else
                target.GetServiceProvider().Damage(_stat.GetAttack());
#endif
            }
            else
                Debug.LogWarning($"몬스터가 {col.collider.gameObject.name}의 컴포넌트를 가지고 올 수 없습니다.");
        }
    }


    public class MonsterBattleStat : IBattleStat {
        Func<int> _attack;
        public MonsterBattleStat(Stat<MonsterStat> stat) {
            _attack = () => stat.Current.Attack;
        }

        public int GetAttack() {
            return _attack();
        }
    }
}