using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightHolySlash : CharacterSkill
    {
        public KnightHolySlash(CharacterServiceProvider characterServiceProvider) : base(characterServiceProvider) { }
        
        protected override void ActivateSkill()
        {
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, GetSkillIndex($"{KnightSkillType.HolySlash}"), null);
            
            base.ActivateSkill();
        }
        
        public override void ActivateSkillEffects()
        {
            Debug.Log("홀리 슬래시!");
            AttackEnemy(GetSkillValue($"{KnightSkillType.HolySlash}") * comboCount);
        }
    }
}