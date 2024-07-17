using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Skills.interfaces;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class HolySlash : CharacterSkill, IKnightSkill, IChangeState
    {
        public KnightSkillType KnightSkillType { get; }
        
        private readonly StateType _targetState;
        
        private Character _character;
        
        public HolySlash(CharacterType characterType, KnightSkillType skillType) : base(characterType)
        {
            KnightSkillType = skillType;
            _targetState = StateType.Skill;
        }
        
        public override void ActivateSkill()
        {
            ChangeState();
        }

        public void ChangeState()
        {
            _character.HFSM.TryChangeState(_targetState);
            // _character.Animator.SetInteger();
        }
    }
}