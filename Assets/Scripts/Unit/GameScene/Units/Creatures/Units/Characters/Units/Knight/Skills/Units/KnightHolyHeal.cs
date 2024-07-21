using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightHolyHeal : CharacterSkill
    {
        public KnightHolyHeal(CharacterServiceProvider characterServiceProvider) : base(characterServiceProvider) { }

        protected override void ActivateSkill(int combo)
        {
            Debug.Log($"{nameof(KnightHolyHeal)} {nameof(ActivateSkill)} 로직 처리");
            
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, GetSkillIndex($"{KnightSkillType.HolyHeal}"), null);
            SetDamageOnBattleSystem(GetSkillValue($"{KnightSkillType.HolyHeal}") * combo);
            
            base.ActivateSkill(combo);
        }
    }
}