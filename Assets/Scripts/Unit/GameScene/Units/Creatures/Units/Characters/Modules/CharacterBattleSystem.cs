using System;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules;
using Unit.GameScene.Units.Creatures.Module.Systems;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;
using Unit.GameScene.Units.Creatures.Units.Monsters;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    public class CharacterBattleSystem : BattleSystem, ICharacterBattle
    {
        public CharacterBattleSystem(Transform targetTransform, IBattleStat battleStat) : base(targetTransform, battleStat) { }

        public override void Attack(RaycastHit2D col)
        {
            if (col.collider.gameObject.TryGetComponent<ICreatureServiceProvider>(out var target))
            {
#if UNITY_EDITOR
                target.HeathSystem.TakeDamage(BattleStat.GetAttack());
                Debug.Log($"플레이어가 {col.collider.gameObject.name}에게 {BattleStat.GetAttack()} 피해를 입혔습니다.");
#else
                target.HeathSystem.TakeDamage(BattleStat.GetAttack());
#endif
            }
            else
            {
                Debug.LogWarning($"플레이어가 {col.collider.gameObject.name}의 컴포넌트를 가지고 올 수 없습니다.");
            }
        }

        public override void Attack(RaycastHit2D col, IBattleEffect effect)
        {
            if (col.collider.gameObject.TryGetComponent<Creature>(out var helath))
            {
                effect.Attack(helath);
            }
        }

        public int GetSkillIndex(string skillName)
        {
            return BattleStat.GetSkillIndex(skillName);
        }

        public int GetSkillValue(string skillName)
        {
            return BattleStat.GetSkillValue(skillName);
        }

        public float GetSkillRange(string skillName)
        {
            return BattleStat.GetSkillValue(skillName);
        }

        public override void Update()
        {
        }
    }

    public class CharacterBattleStat : IBattleStat
    {
        private readonly Func<int> _attack;
        private readonly Func<float> _cool;
        private readonly Func<string, float> _skillRange;
        private readonly Func<string, int> _skillIndex;
        private readonly Func<string, int> _skillValue;

        private CharacterStatSystem _characterStatSystem;

        public CharacterBattleStat(CreatureStat<CharacterStat> creatureStat, CharacterData characterData)
        {
            _attack = () => creatureStat.Current.Damage;
            _cool = () => creatureStat.Current.CoolTime;
            _skillRange = (skillName) => characterData.SkillSystem.GetSkillRange1(skillName);
            _skillIndex = (skillName) => characterData.SkillSystem.GetSkillIndex(skillName);
            _skillValue = (skillName) => characterData.SkillSystem.GetSkillValue(skillName);
        }

        public int GetAttack()
        {
            return _attack();
        }

        public float GetCoolTime()
        {
            return _cool();
        }

        public int GetSkillIndex(string skillName)
        {
            return _skillIndex(skillName);
        }

        public int GetSkillValue(string skillName)
        {
            return _skillValue(skillName);
        }

        public float GetSkillRange(string skillName)
        {
            return _skillRange(skillName);
        }
    }
}