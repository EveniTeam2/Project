using ScriptableObjects.Scripts.Creature.Data.KnightData;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightHolyHeal : CharacterSkill
    {
        public KnightHolyHeal(KnightSkillData knightSkillData)
        {
            SkillName = $"{knightSkillData.skillName}";
            Icon = knightSkillData.SkillIcon;
        }

        protected override void ActivateSkill()
        {
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, GetSkillIndex($"{KnightSkillType.HolyHeal}"), null);
            
            base.ActivateSkill();
        }
        
        public override void ActivateSkillEffects()
        {
            Debug.Log("홀리 힐!");
            HealMyself(GetSkillValue($"{KnightSkillType.HolyHeal}") * ComboCount);
        }
    }
}