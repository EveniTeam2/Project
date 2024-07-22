using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightSwordBuff : CharacterSkill
    {
        public KnightSwordBuff(CharacterServiceProvider characterServiceProvider) : base(characterServiceProvider) {}

        protected override void ActivateSkill()
        {
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, GetSkillIndex($"{KnightSkillType.SwordBuff}"), null);

            base.ActivateSkill();
        }

        public override void ActivateSkillEffects()
        {
            Debug.Log("소드 버프!");
            AttackEnemy(GetSkillValue($"{KnightSkillType.SwordBuff}") * comboCount);
        }
    }
}