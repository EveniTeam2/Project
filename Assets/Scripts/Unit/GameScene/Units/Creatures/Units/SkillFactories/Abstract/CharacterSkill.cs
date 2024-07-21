using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
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
            ActivateSkill(combo);
        }

        protected virtual void ActivateSkill(int combo)
        {
            CharacterServiceProvider.RegistEventSkill(
                () => HandleOnEnter(combo),
                () => HandleOnExit(combo),
                () => HandleOnUpdate(combo),
                () => HandleOnFixedUpdate(combo));
            
            ChangeState(StateType.Skill);
            SetBoolOnAnimator(AnimationParameterEnums.Skill, true, HandleOnAnimationFinished);
        }
        
        protected virtual void HandleOnEnter(int combo) { }
        protected virtual void HandleOnUpdate(int combo) { }
        protected virtual void HandleOnFixedUpdate(int combo) { }
        protected virtual void HandleOnExit(int combo) { }

        private void HandleOnAnimationFinished()
        {
            SetBoolOnAnimator(AnimationParameterEnums.Skill, false, null);
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, -1, null);
            
            ChangeState(StateType.Idle);
            SetReadyForInvokingCommand(true);
        }
        
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