using Assets.Scripts.Unit.GameScene.Units.Creatures.Units;
using System;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Monsters;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    public class CharacterBattleSystem : BattleSystem, ICharacterBattle
    {
        public CharacterBattleSystem(Transform targetTransform, CharacterBattleStat stat) : base(targetTransform, stat) { }

        public override void Attack(RaycastHit2D col)
        {
            if (col.collider.gameObject.TryGetComponent<ICreatureServiceProvider>(out var target))
            {
#if UNITY_EDITOR
                target.HeathSystem.TakeDamage(_stat.GetAttack());
                Debug.Log($"플레이어가 {col.collider.gameObject.name}에게 {_stat.GetAttack()} 피해를 입혔습니다.");
#else
                var dmg = target.GetServiceProvider().TakeDamage(_stat.GetAttack());
#endif
            }
            else
            {
                Debug.LogWarning($"플레이어가 {col.collider.gameObject.name}의 컴포넌트를 가지고 올 수 없습니다.");
            }
        }

        public override void Attack(RaycastHit2D col, IBattleEffect effect)
        {
            //TODO : 채이환
        }

        public int GetSkillIndex(string skillName)
        {
            // TODO : 인호님 이거 고쳐주세요.
            return _stat.GetSkillIndex(skillName);
        }

        public int GetSkillValue(string skillName)
        {
            // TODO : 인호님 이거 고쳐주세요.
            return 0;
        }

        public float GetSkillRange(string skillName)
        {
            // TODO : 인호님 이거 고쳐주세요.
            return 0;
        }

        public override void Update()
        {
        }
    }

    public class CharacterBattleStat : IBattleStat
    {
        private readonly Func<int> _attack;
        private readonly Func<float> _cool;

        private CharacterData _characterData;
        
        public CharacterBattleStat(CreatureStat<CharacterStat> creatureStat, CharacterData characterData)
        {
            _attack = () => creatureStat.Current.Damage;
            _cool = () => creatureStat.Current.CoolTime;
            // TODO : 인호님 이거 고쳐주세요.
            characterData.SkillManager.GetSkillRange("");
        }

        public int GetAttack()
        {
            return _attack();
        }

        public float GetCoolTime()
        {
            return _cool();
        }

        public int GetSkillIndex()
        {
            // TODO : 인호님 이거 고쳐주세요.
            return 0;
        }

        public int GetSkillIndex(string skillName)
        {
            // TODO : 인호님 이거 고쳐주세요.
            return 0;
        }
    }
}