using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightBaseAttack : CharacterSkill
    {
        public KnightBaseAttack(CharacterServiceProvider characterServiceProvider) : base(characterServiceProvider) { }

        protected override void HandleOnEnter(int combo)
        {
            base.HandleOnEnter(combo);
            
            Debug.Log($"{nameof(KnightBaseAttack)} Action");
            
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, GetSkillIndex($"{KnightSkillType.BaseAttack}"));
            SetDamageOnBattleSystem(GetSkillValue($"{KnightSkillType.BaseAttack}") * combo);
        }

        protected override void HandleOnUpdate(int combo) { }

        protected override void HandleOnFixedUpdate(int combo) { }
        
        protected override void HandleOnExit(int combo)
        {
            base.HandleOnExit(combo);
            
            ChangeState(StateType.Run);
        }
    }
}