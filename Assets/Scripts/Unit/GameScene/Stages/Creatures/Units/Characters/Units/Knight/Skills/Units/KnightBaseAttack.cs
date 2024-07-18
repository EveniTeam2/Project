using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightBaseAttack : CharacterSkill, IChangeState
    {
        protected override void HandleDefineSkill(int combo)
        {
            Debug.Log($"{nameof(CharacterSkill)} - {nameof(KnightBaseAttack)} x {combo}");
            ChangeState();
        }

        public void ChangeState()
        {
            ServiceProvider.TryChangeState(StateType.Skill);
            // var skillIndex = ServiceProvider.GetSkillIndex($"{KnightSkillType.BaseAttack}");
            // ServiceProvider.AnimatorSetBool(AnimationParameterEnums.Skill, true);
            // ServiceProvider.AnimatorSetInteger(AnimationParameterEnums.Skill, skillIndex);
        }
    }
}