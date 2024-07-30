using ScriptableObjects.Scripts.Creature.Data.KnightData;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightSwordBuff : CharacterSkill
    {
        public KnightSwordBuff(KnightSkillData knightSkillData)
        {
            SkillName = $"{knightSkillData.skillName}";
            Icon = knightSkillData.SkillIcon;
        }

        protected override void ActivateSkill()
        {
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, GetSkillIndex($"{KnightSkillType.SwordBuff}"), null);
            
            base.ActivateSkill();
        }

        public override void ActivateSkillEffects()
        {
            Debug.Log("소드 버프!");
            AttackEnemy(GetSkillValue(SkillName) * ComboCount, GetSkillRange(SkillName));
        }
    }
}