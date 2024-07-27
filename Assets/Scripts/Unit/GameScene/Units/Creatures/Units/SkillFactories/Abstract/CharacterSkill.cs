using System;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract
{
    public abstract class CharacterSkill : ISkillCommand
    {
        public string SkillName { get; protected set; }
        public int SkillLevel { get; protected set; } = 1;
        public Sprite Icon { get; protected set; }
        
        protected int MatchBlockCount;
        protected int ComboCount;
        
        private CharacterServiceProvider _characterServiceProvider;

        public void Execute(int combo)
        {
            ComboCount = combo;
            
            ActivateSkill();
        }
        
        
        public void IncreaseLevel()
        {
            SkillLevel++;
        }

        public void RegisterCharacterServiceProvider(CharacterServiceProvider characterServiceProvider)
        {
            _characterServiceProvider = characterServiceProvider;
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
            _characterServiceProvider.AnimatorSetBool(targetParameter, value, action);
        }

        private void SetReadyForInvokingCommand(bool isReady)
        {
            _characterServiceProvider.SetReadyForCommand(isReady);
        }

        protected void ChangeState(StateType targetState)
        {
            _characterServiceProvider.TryChangeState(targetState);
        }

        protected void SetFloatOnAnimator(AnimationParameterEnums targetParameter, int value, Action action)
        {
            _characterServiceProvider.AnimatorSetFloat(targetParameter, value, action);
        }

        protected void AttackEnemy(int value, float range)
        {
            _characterServiceProvider.AttackEnemy(value, range);
        }

        protected void HealMyself(int value)
        {
            _characterServiceProvider.HealMyself(value);
        }

        protected int GetSkillIndex(string skillName)
        {
            return _characterServiceProvider.GetSkillIndex(skillName);
        }

        protected int GetSkillValue(string skillName)
        {
            return _characterServiceProvider.GetSkillValue(skillName);
        }
        
        protected float GetSkillRange(string skillName)
        {
            return _characterServiceProvider.GetSkillRange(skillName);
        }
    }
}