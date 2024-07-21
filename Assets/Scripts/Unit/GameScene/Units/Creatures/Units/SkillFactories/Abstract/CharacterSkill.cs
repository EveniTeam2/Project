using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract
{
    public abstract class CharacterSkill : Skill, ISkillCommand
    {
        protected readonly CharacterServiceProvider CharacterServiceProvider;

        protected CharacterSkill(CharacterServiceProvider characterServiceProvider)
        {
            CharacterServiceProvider = characterServiceProvider;
        }
        
        public void Execute(int combo)
        {
            CharacterServiceProvider.RegistEventSkill(
                () => HandleOnEnter(combo),
                () => HandleOnExit(combo),
                () => HandleOnUpdate(combo), 
                () => HandleOnFixedUpdate(combo));
            
            ChangeState(StateType.Skill);
        }

        protected virtual void HandleOnEnter(int combo)
        {
            SetBoolOnAnimator(AnimationParameterEnums.Skill, true);
        }

        protected virtual void HandleOnExit(int combo)
        {
            SetBoolOnAnimator(AnimationParameterEnums.Skill, false);
        }

        protected virtual void HandleOnUpdate(int combo)
        {
            if (!CharacterServiceProvider.GetSkillAnimationState())
            {
                ChangeState(StateType.Run);
            }
        }
        
        protected abstract void HandleOnFixedUpdate(int combo);

        protected void SetBoolOnAnimator(AnimationParameterEnums targetParameter, bool value)
        {
            CharacterServiceProvider.AnimatorSetBool(targetParameter, value);
        }

        protected void ChangeState(StateType targetState)
        {
            CharacterServiceProvider.TryChangeState(targetState);
        }

        protected void SetFloatOnAnimator(AnimationParameterEnums targetParameter, int value)
        {
            CharacterServiceProvider.AnimatorSetFloat(targetParameter, value);
        }

        protected void SetDamageOnBattleSystem(float damage)
        {
            CharacterServiceProvider.AttackEnemy(damage);
        }

        protected int GetSkillIndex(string skillName)
        {
            return CharacterServiceProvider.GetSkillIndex(skillName);
        }

        protected float GetSkillValue(string skillName)
        {
            return CharacterServiceProvider.GetSkillValue(skillName);
        }
    }
}