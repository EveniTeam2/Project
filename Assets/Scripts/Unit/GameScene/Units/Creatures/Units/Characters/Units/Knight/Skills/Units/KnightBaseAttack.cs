using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightBaseAttack : CharacterSkill
    {
        public KnightBaseAttack(CharacterServiceProvider characterServiceProvider) : base(characterServiceProvider) { }

        protected override void ActivateSkill()
        {
            Debug.Log($"{nameof(KnightBaseAttack)} {nameof(ActivateSkill)} 로직 처리");
            
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, GetSkillIndex($"{KnightSkillType.BaseAttack}"), null);
            
            base.ActivateSkill();
        }

        public override void ActivateSkillEffects()
        {
            Debug.Log("일반 공격!");
            AttackEnemy(GetSkillValue($"{KnightSkillType.BaseAttack}") * comboCount);
        }
    }
}