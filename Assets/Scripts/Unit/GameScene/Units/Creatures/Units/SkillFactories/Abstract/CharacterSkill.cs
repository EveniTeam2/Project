using System;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract
{
    public abstract class CharacterSkill : Skill, ISkillCommand
    {
        protected int comboCount;
        protected readonly CharacterServiceProvider CharacterServiceProvider;

        protected CharacterSkill(CharacterServiceProvider characterServiceProvider)
        {
            CharacterServiceProvider = characterServiceProvider;
        }
        
        public void Execute(int combo)
        {
            comboCount = combo;
            
            ActivateSkill();
        }

        protected virtual void ActivateSkill()
        {
            ChangeState(StateType.Skill);
            SetBoolOnAnimator(AnimationParameterEnums.Skill, true, HandleOnAnimationFinished);
        }
        
        private void HandleOnAnimationFinished()
        {
            SetBoolOnAnimator(AnimationParameterEnums.Skill, false, null);
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, -1, null);
            
            ChangeState(StateType.Idle);
            SetReadyForInvokingCommand(true);
        }

        public abstract void ActivateSkillEffects();
        
        // CharacterServiceProvider.RegisterEventSkill(
        //     () => HandleOnEnter(combo),
        //     () => HandleOnExit(combo),
        //     () => HandleOnUpdate(combo),
        //     () => HandleOnFixedUpdate(combo));
        
        // protected virtual void HandleOnEnter(int combo) { }
        // protected virtual void HandleOnUpdate(int combo) { }
        // protected virtual void HandleOnFixedUpdate(int combo) { }
        // protected virtual void HandleOnExit(int combo) { }


        
        protected void SetBoolOnAnimator(AnimationParameterEnums targetParameter, bool value, Action action)
        {
            CharacterServiceProvider.AnimatorSetBool(targetParameter, value, action);
        }

        private void SetReadyForInvokingCommand(bool isReady)
        {
            CharacterServiceProvider.SetReadyForCommand(isReady);
        }

        protected void ChangeState(StateType targetState)
        {
            CharacterServiceProvider.TryChangeState(targetState);
        }

        protected void SetFloatOnAnimator(AnimationParameterEnums targetParameter, int value, Action action)
        {
            CharacterServiceProvider.AnimatorSetFloat(targetParameter, value, action);
        }

        protected void AttackEnemy(float value)
        {
            CharacterServiceProvider.AttackEnemy(value);
        }

        protected void HealMyself(float value)
        {
            CharacterServiceProvider.AttackEnemy(value);
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