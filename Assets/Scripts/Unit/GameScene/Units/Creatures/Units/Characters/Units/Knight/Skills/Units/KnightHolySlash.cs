using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightHolySlash : CharacterSkill
    {
        private readonly int _skillIndex;
        
        public KnightHolySlash(CharacterServiceProvider characterServiceProvider) : base(characterServiceProvider) { }
        
        protected override void ActivateSkill(int combo)
        {
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, GetSkillIndex($"{KnightSkillType.HolySlash}"), null);
            SetDamageOnBattleSystem(GetSkillValue($"{KnightSkillType.HolySlash}") * combo);
            
            base.ActivateSkill(combo);
        }
    }
}