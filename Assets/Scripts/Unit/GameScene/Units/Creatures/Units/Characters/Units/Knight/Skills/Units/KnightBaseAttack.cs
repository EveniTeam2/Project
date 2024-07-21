using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightBaseAttack : CharacterSkill
    {
        public KnightBaseAttack(CharacterServiceProvider characterServiceProvider) : base(characterServiceProvider) { }

        protected override void ActivateSkill(int combo)
        {
            Debug.Log($"{nameof(KnightBaseAttack)} {nameof(ActivateSkill)} 로직 처리");
            
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, GetSkillIndex($"{KnightSkillType.BaseAttack}"), null);
            SetDamageOnBattleSystem(GetSkillValue($"{KnightSkillType.BaseAttack}") * combo);
            
            base.ActivateSkill(combo);
        }
    }
}