using System;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
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
        
        private ICharacterServiceProvider _characterServiceProvider;

        public void Execute(int combo)
        {
            ComboCount = combo;
            
            ActivateSkill();
        }
        
        
        public void IncreaseLevel()
        {
            SkillLevel++;
        }

        public void RegisterCharacterServiceProvider(ICharacterServiceProvider creatureServiceProvider)
        {
            _characterServiceProvider = creatureServiceProvider;
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
            _characterServiceProvider.creatureServiceProvider.AnimatorSystem.SetBool(targetParameter, value, action);
        }

        private void SetReadyForInvokingCommand(bool isReady)
        {
            _characterServiceProvider.SetReadyForCommand(isReady);
        }

        protected void ChangeState(StateType targetState)
        {
            _characterServiceProvider.creatureServiceProvider.StateSystem.TryChangeState(targetState);
        }

        protected void SetFloatOnAnimator(AnimationParameterEnums targetParameter, int value, Action action)
        {
            _characterServiceProvider.creatureServiceProvider.AnimatorSystem.SetFloat(targetParameter, value, action);
        }

        protected void AttackEnemy(int value, float range)
        {
            _characterServiceProvider.BattleSystem.Attack(value, range);
        }

        protected void HealMyself(int value)
        {
            _characterServiceProvider.HeathSystem.TakeHeal(value);
        }

        protected int GetSkillIndex(string skillName)
        {
            return _characterServiceProvider.BattleSystem.GetSkillIndex(skillName);
        }

        protected int GetSkillValue(string skillName)
        {
            return _characterServiceProvider.BattleSystem.GetSkillValue(skillName);
        }
        
        protected float GetSkillRange(string skillName)
        {
            return _characterServiceProvider.BattleSystem.GetSkillRange(skillName);
        }
    }
}