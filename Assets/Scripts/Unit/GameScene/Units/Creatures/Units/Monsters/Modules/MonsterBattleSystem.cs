using System;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Module.Systems;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;
using Unit.GameScene.Units.Creatures.Units.Characters;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules
{
    public class MonsterBattleSystem : BattleSystem
    {
        protected float timer;

        public MonsterBattleSystem(Transform targetTransform, MonsterBattleStat battleStat) : base(targetTransform, battleStat)
        {
        }

        public override void Attack(RaycastHit2D col)
        {
            IsReadyForAttack = false;
            timer = BattleStat.GetCoolTime();

            if (col.collider.gameObject.TryGetComponent<ICreatureServiceProvider>(out var target))
            {
#if UNITY_EDITOR
                target.HeathSystem.TakeDamage(BattleStat.GetAttack());
                Debug.Log($"몬스터가 {col.collider.gameObject.name}에게 {BattleStat.GetAttack()} 피해를 입혔습니다.");
#else
                var dmg = target.GetServiceProvider().TakeDamage(_stat.GetAttack());
#endif
            }
            else
            {
                Debug.LogWarning($"몬스터가 {col.collider.gameObject.name}의 컴포넌트를 가지고 올 수 없습니다.");
            }
        }

        public override void Attack(RaycastHit2D col, IBattleEffect effect)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            if (!IsReadyForAttack)
            {
                timer -= Time.deltaTime;
                if (timer < 0) IsReadyForAttack = true;
            }
        }
    }


    public class MonsterBattleStat : IBattleStat
    {
        private readonly Func<int> _attack;
        private readonly Func<float> _coolTime;

        public MonsterBattleStat(CreatureStat<MonsterStat> creatureStat)
        {
            _attack = () => creatureStat.Current.Attack;
            _coolTime = () => creatureStat.Current.CoolTime;
        }

        public float GetCoolTime()
        {
            return _coolTime();
        }

        public int GetAttack()
        {
            return _attack();
        }

        //TODO : 채이환 - 이환님 이 부분 하려면 MonsterData 안에 SkillManager가 존재해야 해요. CharacterBattleSystem 수정해뒀어요. 이 부분 참고하셔서 작업해주세요.
        public int GetSkillIndex(string skillName)
        {
            return 0;
        }

        public float GetSkillRange(string skillName)
        {
            return 0;
        }

        public int GetSkillValue(string skillName)
        {
            return 0;
        }
    }
}