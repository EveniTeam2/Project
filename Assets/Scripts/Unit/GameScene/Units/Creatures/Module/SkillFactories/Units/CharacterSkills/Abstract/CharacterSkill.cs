using System;
using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Interfaces;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills.Abstract
{
    public abstract class CharacterSkill : ISkillCommand
    {
        public CharacterClassType CharacterClassType { get; private set; }
        public SkillEffectType SkillEffectType { get; private set; }
        public string SkillName { get; protected set; }
        public int SkillIndex { get; protected set; }
        public int SkillLevel { get; private set; } = 1;
        public Sprite Icon { get; protected set; }
        
        protected int MatchBlockCount;
        protected int ComboCount;
        
        private ISkillController _skillController;
        private readonly List<SkillData> _csvData;

        protected CharacterSkill(List<SkillData> csvData)
        {
            _csvData = csvData;

            CharacterClassType = _csvData[0].CharacterType;
            SkillEffectType = _csvData[0].SkillEffectType;
            SkillName = _csvData[0].SkillName;
            SkillIndex = _csvData[0].SkillIndex;
        }
        
        public abstract void ActivateSkillEffects();

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
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, SkillIndex, null);
            SetBoolOnAnimator(AnimationParameterEnums.Skill, true, HandleOnAnimationFinished);
        }
        
        private void HandleOnAnimationFinished()
        {
            SetBoolOnAnimator(AnimationParameterEnums.Skill, false, null);
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, -1, null);
            
            ChangeState(StateType.Idle);
            SetReadyForInvokingCommand(true);
        }
        
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

        protected int GetSkillEffectValue()
        {
            return (from data in _csvData where data.SkillIndex == SkillIndex && data.SkillLevel == SkillLevel select data.SkillEffectValue).FirstOrDefault();
        }
        
        protected float GetSkillRange()
        {
            return (from data in _csvData where data.SkillIndex == SkillIndex && data.SkillLevel == SkillLevel select data.SkillRange).FirstOrDefault();
        }
    }
}