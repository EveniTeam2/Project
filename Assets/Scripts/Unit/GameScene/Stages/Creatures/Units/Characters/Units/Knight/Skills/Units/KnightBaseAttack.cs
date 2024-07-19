using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightBaseAttack : CharacterSkill, ISkillCommand, IChangeState, ISetFloatParameter
    {
        public KnightBaseAttack(CharacterServiceProvider characterServiceProvider) : base(characterServiceProvider) { }

        public void Execute(int combo)
        {
            Debug.Log($"{nameof(CharacterSkill)} - {nameof(KnightBaseAttack)} x {combo}");

            SetFloatParameter();
            ChangeState();
        }
        
        public void ChangeState()
        {
            CharacterServiceProvider.TryChangeState(StateType.Skill);
        }

        public void SetFloatParameter()
        {
            var skillIndex = CharacterServiceProvider.GetSkillIndex($"{KnightSkillType.BaseAttack}");
            CharacterServiceProvider.AnimatorSetFloat(AnimationParameterEnums.SkillIndex, skillIndex);
            
            Debug.Log($"{nameof(skillIndex)} {skillIndex}");
        }
    }
}