using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightHolySlash : CharacterSkill, ISkillCommand, IChangeState, ISetFloatParameter
    {
        public KnightHolySlash(CharacterServiceProvider characterServiceProvider) : base(characterServiceProvider) { }
        
        public void Execute(int comboCount)
        {
            Debug.Log($"{nameof(CharacterSkill)} - {nameof(KnightHolySlash)} x {comboCount}");

            SetFloatParameter();
            ChangeState();
        }
        
        public void ChangeState()
        {
            CharacterServiceProvider.TryChangeState(StateType.Skill);
        }

        public void SetFloatParameter()
        {
            var skillIndex = CharacterServiceProvider.GetSkillIndex($"{KnightSkillType.HolySlash}");
            CharacterServiceProvider.AnimatorSetFloat(AnimationParameterEnums.SkillIndex, skillIndex);
            
            Debug.Log($"{nameof(skillIndex)} {skillIndex}");
        }
    }
}