using System;
using System.Collections.Generic;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Modules
{
    public class CharacterServiceProvider : ICreatureServiceProvider
    {
        private readonly CharacterType _characterType;
        private readonly Animator _animator;
        private readonly BattleSystem _battleSystem;
        private readonly StateMachine _fsm;
        private readonly HealthSystem _healthSystem;
        private readonly MovementSystem _movementSystem;
        private readonly Dictionary<AnimationParameterEnums, int> _animationParameter;
        private readonly Dictionary<string, int> _skillIndexes;
        private readonly Dictionary<string, float> _skillValues;
        private bool _skillTransition;

        public CharacterServiceProvider(
            CharacterType characterType,
            BattleSystem battleSystem,
            HealthSystem healthSystem,
            MovementSystem movementSystem,
            Animator animator,
            StateMachine fsm,
            Dictionary<AnimationParameterEnums, int> animationParameter,
            Dictionary<string, int> skillIndexes,
            Dictionary<string, float> skillValues)
        {
            _characterType = characterType;
            _battleSystem = battleSystem;
            _healthSystem = healthSystem;
            _movementSystem = movementSystem;
            _animator = animator;
            _fsm = fsm;
            _animationParameter = animationParameter;
            _skillIndexes = skillIndexes;
            _skillValues = skillValues;
        }

        public void SetSkillAnimationState(bool value)
        {
            _skillTransition = value;
        }

        public bool GetSkillAnimationState()
        {
            return _skillTransition;
        }

        public float GetSkillValue(string skillName)
        {
            return _skillValues[skillName];
        }

        public int GetSkillIndex(string skillName)
        {
            return _skillIndexes[skillName];
        }

        public void AttackEnemy(float damage)
        {
            // TODO : damage 주면 몬스터 때찌해주세요 (채이환)
        }

        public void AnimatorSetInteger(AnimationParameterEnums parameter, int value)
        {
            _animator.SetInteger(_animationParameter[parameter], value);
        }

        public void AnimatorSetFloat(AnimationParameterEnums parameter, float value)
        {
            _animator.SetFloat(_animationParameter[parameter], value);
        }

        public void AnimatorSetBool(AnimationParameterEnums parameter, bool value)
        {
            _animator.SetBool(_animationParameter[parameter], value);
        }

        public int TakeDamage(int atk)
        {
            _healthSystem.Damage(atk);
            return atk;
        }

        public AnimatorStateInfo GetCurrentAnimatorStateInfo()
        {
            return _animator.GetCurrentAnimatorStateInfo(0);
        }

        public AnimatorStateInfo GetNextAnimatorStateInfo()
        {
            return _animator.GetNextAnimatorStateInfo(0);
        }

        public void RegisterEvent(ECharacterEventType type, Action subscriber)
        {
            switch (type)
            {
                case ECharacterEventType.Death:
                    _healthSystem.RegistOnDeathEvent(subscriber);
                    break;
                case ECharacterEventType.Damage:
                    _healthSystem.RegistOnDamageEvent(subscriber);
                    break;
            }
        }

        public void Run(bool isRun)
        {
            _movementSystem.SetRun(isRun);
        }

        public bool TryChangeState(StateType stateType)
        {
            return _fsm.TryChangeState(stateType);
        }

        public void RegistEventSkill(Action OnEnter, Action OnExit, Action OnUpdate, Action OnFixedUpdate)
        {
            _fsm.RegistOnSkillState(OnEnter, OnExit, OnUpdate, OnFixedUpdate);
        }

        //public bool TryChangeState(StateType stateType, Action onExecute)
        //{
        //    return _fsm.TryChangeState(stateType, onExecute);
        //}
    }
}