using System;
using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.SkillFactories.Interfaces;
using Unit.GameScene.Units.SkillFactories.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract
{
    public class CharacterSkill : ISkillCommand
    {
        public CharacterClassType CharacterClassType { get; private set; }
        public SkillType SkillType { get; private set; }
        public string SkillName { get; private set; }
        public string SkillDescription { get; private set; }
        public int SkillIndex { get; private set; }
        public int SkillLevel { get; private set; } = 1;
        public Sprite SkillIcon { get; private set; }
        
        protected int MatchBlockCount;
        protected int ComboCount;
        
        private ICharacterSkillController _characterSkillController;
        private List<SkillData> _csvData;
        
        public int GetSkillIndex() => SkillIndex;
        public int GetSkillValue() => (from data in _csvData where data.SkillIndex == SkillIndex && data.SkillLevel == SkillLevel select data.SkillValue).FirstOrDefault();
        public float GetSkillRange1() => (from data in _csvData where data.SkillIndex == SkillIndex && data.SkillLevel == SkillLevel select data.SkillRange1).FirstOrDefault();
        public float GetSkillRange2() => (from data in _csvData where data.SkillIndex == SkillIndex && data.SkillLevel == SkillLevel select data.SkillRange2).FirstOrDefault();

        public void ActivateSkillEffects()
        {
            switch (SkillType)
            {
                case SkillType.Attack:
                    AttackEnemy(GetSkillValue() * ComboCount, GetSkillRange1());
                    break;
                case SkillType.Heal:
                    HealMyself(GetSkillValue() * ComboCount);
                    break;
                case SkillType.BuffDamage:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void Initialize(Sprite skillIcon, List<SkillData> csvData)
        {
            _csvData = csvData;

            var initialData = _csvData[0];
            
            SkillIcon = skillIcon;
            CharacterClassType = initialData.CharacterType;
            SkillType = initialData.SkillType;
            SkillName = initialData.SkillName;
            SkillDescription = initialData.SkillDescription;
            SkillIndex = initialData.SkillIndex;
        }

        public void Execute(int combo)
        {
            ComboCount = combo;
            
            ActivateSkill();
        }
        
        public void IncreaseLevel()
        {
            SkillLevel++;
        }

        public void RegisterCharacterServiceProvider(ICharacterSkillController characterSkillController)
        {
            _characterSkillController = characterSkillController;
        }
        
        protected void AttackEnemy(int value, float range)
        {
            _characterSkillController.AttackEnemy(value, range);
        }

        protected void HealMyself(int value)
        {
            _characterSkillController.HealMySelf(value);
        }

        protected void Summon()
        {
            _characterSkillController.Summon();
        }
        
        private void ActivateSkill()
        {
            ChangeState(StateType.Skill);
            
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, SkillIndex, null);
            SetBoolOnAnimator(AnimationParameterEnums.Skill, true, HandleOnAnimationFinished);
        }
        
        private void HandleOnAnimationFinished()
        {
            SetBoolOnAnimator(AnimationParameterEnums.Skill, false, null);
            SetFloatOnAnimator(AnimationParameterEnums.SkillIndex, -1, null);
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
    }
}