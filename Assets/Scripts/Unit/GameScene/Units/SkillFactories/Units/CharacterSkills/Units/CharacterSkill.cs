using System;
using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.Battles;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.SkillFactories.Interfaces;
using Unit.GameScene.Units.SkillFactories.Modules;
using UnityEngine;
using ICharacterSkillController = Unit.GameScene.Units.Creatures.Interfaces.Battles.ICharacterSkillController;

namespace Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units
{
    public class CharacterSkill : ISkillCommandAction
    {
        private event Action OnIncreaseSkillLevel;
        
        public CharacterType CharacterType { get; private set; }
        public SkillType SkillType { get; private set; }
        public Sprite SkillIcon { get; private set; }
        public string SkillName { get; private set; }
        public int SkillIndex { get; private set; }
        public int SkillCurrentLevel { get; private set; }
        public int SkillMaxLevel { get; private set; }
        public bool IsRegisterOnBlock { get; internal set; }

        protected int MatchBlockCount;
        protected int ComboCount;
        
        private ICharacterSkillController _characterSkillController;
        private List<SkillData> _csvData;
        private Action _increaseLevel;
        
        public int GetSkillIndex() => SkillIndex;
        public int GetSkillValue() => (from data in _csvData where data.SkillIndex == SkillIndex && data.SkillLevel == SkillCurrentLevel select data.SkillValue).FirstOrDefault();
        public float GetSkillDuration() => (from data in _csvData where data.SkillIndex == SkillIndex && data.SkillLevel == SkillCurrentLevel select data.SkillDuration).FirstOrDefault();
        public float GetSkillRange1() => (from data in _csvData where data.SkillIndex == SkillIndex && data.SkillLevel == SkillCurrentLevel select data.SkillRange1).FirstOrDefault();
        public float GetSkillRange2() => (from data in _csvData where data.SkillIndex == SkillIndex && data.SkillLevel == SkillCurrentLevel select data.SkillRange2).FirstOrDefault();
        public string GetSkillDescription() => (from data in _csvData where data.SkillIndex == SkillIndex && data.SkillLevel == SkillCurrentLevel select data.SkillDescription).FirstOrDefault();
        public string GetNextLevelSkillDescription() => (from data in _csvData where data.SkillIndex == SkillIndex && data.SkillLevel == SkillCurrentLevel + 1 select data.SkillDescription).FirstOrDefault();
        public int GetNextLevel() => SkillCurrentLevel + 1;
        
        public void Initialize(Sprite skillIcon, List<SkillData> csvData)
        {
            _csvData = csvData;

            var initialData = _csvData[0];
            
            SkillIcon = skillIcon;
            CharacterType = initialData.CharacterType;
            SkillType = initialData.SkillType;
            SkillName = initialData.SkillName;
            SkillIndex = initialData.SkillIndex;
            SkillMaxLevel = csvData.Count;

            SkillCurrentLevel = SkillIndex == 0 ? 1 : 0;

            OnIncreaseSkillLevel += HandleOnIncreaseSkillLevel;
        }
        
        public void ActivateSkillEffects()
        {
            switch (SkillType)
            {
                case SkillType.Attack:
                    _characterSkillController.Attack(GetSkillValue() * ComboCount, GetSkillRange1());
                    break;
                case SkillType.Heal:
                    _characterSkillController.Heal(GetSkillValue() * ComboCount);
                    break;
                case SkillType.BuffDamage:
                    _characterSkillController.Buff(StatType.Damage, GetSkillValue(), GetSkillDuration());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Execute(int combo)
        {
            ComboCount = combo;
            
            ActivateSkill();
        }

        public void InvokeOnIncreaseSkillLevelAction()
        {
            OnIncreaseSkillLevel.Invoke();
        }

        public void RegisterCharacterServiceProvider(ICharacterSkillController characterSkillController)
        {
            _characterSkillController = characterSkillController;
        }
        
        private void ActivateSkill()
        {
            ChangeState(StateType.Skill);
            
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, SkillIndex, null);
            SetBoolOnAnimator(AnimationParameterEnums.Skill, true, HandleOnAnimationFinished);
        }
        
        private void HandleOnAnimationFinished()
        {
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, -1, null);
            SetBoolOnAnimator(AnimationParameterEnums.Skill, false, null);
            SetReadyForInvokingCommand(true);
            
            ChangeState(StateType.Idle);
        }

        private void SetReadyForInvokingCommand(bool isReady)
        {
            _characterSkillController.SetReadyForInvokingCommand(isReady);
        }
        
        private void ChangeState(StateType targetState)
        {
            _characterSkillController.TryChangeState(targetState);
        }
        
        private void SetBoolOnAnimator(AnimationParameterEnums targetParameter, bool value, Action action)
        {
            _characterSkillController.SetBoolOnAnimator(targetParameter, value, action);
        }

        private void SetFloatOnAnimator(AnimationParameterEnums targetParameter, int value, Action action)
        {
            _characterSkillController.SetFloatOnAnimator(targetParameter, value, action);
        }
        
        private void HandleOnIncreaseSkillLevel()
        {
            SkillCurrentLevel++;
        }
    }
}