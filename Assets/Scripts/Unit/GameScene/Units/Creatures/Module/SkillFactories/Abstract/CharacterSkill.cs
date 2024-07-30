using System;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Interfaces;
using Unit.GameScene.Units.Creatures.Units.Characters;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Abstract
{
    public abstract class CharacterSkill : ISkillCommand
    {
        public string SkillName { get; protected set; }
        public int SkillLevel { get; protected set; } = 1;
        public Sprite Icon { get; protected set; }
        
        protected int MatchBlockCount;
        protected int ComboCount;
        
        private ISkillController _skillController;

        public void Execute(int combo)
        {
            ComboCount = combo;
            
            ActivateSkill();
        }
        
        
        public void IncreaseLevel()
        {
            SkillLevel++;
        }

        public void RegisterCharacterServiceProvider(ISkillController skillController)
        {
            _skillController = skillController;
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
        
        protected void SetBoolOnAnimator(AnimationParameterEnums targetParameter, bool value, Action action)
        {
            _skillController.SetBoolOnAnimator(targetParameter, value, action);
        }

        private void SetReadyForInvokingCommand(bool isReady)
        {
            _skillController.SetReadyForInvokingCommand(isReady);
        }

        protected void ChangeState(StateType targetState)
        {
            _skillController.TryChangeState(targetState);
        }

        protected void SetFloatOnAnimator(AnimationParameterEnums targetParameter, int value, Action action)
        {
            _skillController.SetFloatOnAnimator(targetParameter, value, action);
        }

        protected void AttackEnemy(int value, float range)
        {
            _skillController.AttackEnemy(value, range);
        }

        protected void HealMyself(int value)
        {
            _skillController.HealMySelf(value);
        }

        protected int GetSkillIndex(string skillName)
        {
            return _skillController.GetSkillIndex(skillName);
        }

        protected int GetSkillValue(string skillName)
        {
            return _skillController.GetSkillValue(skillName);
        }
        
        protected float GetSkillRange(string skillName)
        {
            return _skillController.GetSkillRange(skillName);
        }
    }
}