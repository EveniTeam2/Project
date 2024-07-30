using ScriptableObjects.Scripts.Creature.Data.KnightData;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightHolySlash : CharacterSkill
    {
        public KnightHolySlash(KnightSkillData knightSkillData)
        {
            SkillName = $"{knightSkillData.skillName}";
            Icon = knightSkillData.SkillIcon;
        }

        protected override void ActivateSkill()
        {
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, GetSkillIndex($"{KnightSkillType.HolySlash}"), null);
            
            base.ActivateSkill();
        }
        
        public override void ActivateSkillEffects()
        {
            AttackEnemy(GetSkillValue(SkillName) * ComboCount, GetSkillRange(SkillName));
        }
    }
}