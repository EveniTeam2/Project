using ScriptableObjects.Scripts.Creature.Data.KnightData;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightLightAttack : CharacterSkill
    {
        public KnightLightAttack(KnightSkillData knightSkillData)
        {
            SkillName = $"{knightSkillData.skillName}";
            Icon = knightSkillData.SkillIcon;
        }

        protected override void ActivateSkill()
        {
            Debug.Log($"{nameof(KnightLightAttack)} {nameof(ActivateSkill)} 로직 처리");
            
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, GetSkillIndex($"{KnightSkillType.LightAttack}"), null);
            
            base.ActivateSkill();
        }

        public override void ActivateSkillEffects()
        {
            Debug.Log("일반 공격!");
            AttackEnemy(GetSkillValue(SkillName) * ComboCount, GetSkillRange(SkillName));
        }
    }
}