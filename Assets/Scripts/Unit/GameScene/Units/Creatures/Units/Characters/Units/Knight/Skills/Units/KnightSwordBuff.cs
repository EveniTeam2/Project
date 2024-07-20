using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightSwordBuff : CharacterSkill, ISkillCommand, IChangeState, ISetFloatParameter
    {
        public KnightSwordBuff(CharacterServiceProvider characterServiceProvider) : base(characterServiceProvider) { }
        
        public void Execute(int comboCount)
        {
            Debug.Log($"{nameof(CharacterSkill)} - {nameof(KnightSwordBuff)} x {comboCount}");
            
            SetFloatParameter();
            ChangeState();
        }

        public void ChangeState()
        {
            CharacterServiceProvider.TryChangeState(StateType.Skill);
        }

        public void SetFloatParameter()
        {
            var skillIndex = CharacterServiceProvider.GetSkillIndex($"{KnightSkillType.SwordBuff}");
            CharacterServiceProvider.AnimatorSetFloat(AnimationParameterEnums.SkillIndex, skillIndex);
            
            Debug.Log($"{nameof(skillIndex)} {skillIndex}");
        }
    }
}