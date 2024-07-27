using System;
using System.Collections.Generic;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    [Serializable]
    public class CharacterServiceProvider : ICreatureServiceProvider
    {
        private readonly CharacterClassType _characterClassType;
        private readonly AnimatorEventReceiver _animatorEventReceiver;
        private readonly BattleSystem _battleSystem;
        private readonly StateMachine _fsm;
        private readonly HealthSystem _healthSystem;
        private readonly MovementSystem _movementSystem;
        private readonly Dictionary<AnimationParameterEnums, int> _animationParameter;
        private readonly SkillManager _skillManager;
        
        private bool _readyForCommand;

        public CharacterServiceProvider(
            CharacterClassType characterClassType,
            BattleSystem battleSystem,
            HealthSystem healthSystem,
            MovementSystem movementSystem,
            AnimatorEventReceiver animatorEventReceiver,
            StateMachine fsm,
            Dictionary<AnimationParameterEnums, int> animationParameter,
            CharacterData characterData)
        {
            _characterClassType = characterClassType;
            _battleSystem = battleSystem;
            _healthSystem = healthSystem;
            _movementSystem = movementSystem;
            _animatorEventReceiver = animatorEventReceiver;
            _fsm = fsm;
            _animationParameter = animationParameter;
            _skillManager = characterData.SkillManager;

            SetReadyForCommand(true);
        }

        public void SetReadyForCommand(bool isReady)
        {
            _readyForCommand = isReady;
        }

        public bool GetReadyForCommand()
        {
            return _readyForCommand;
        }

        public int GetSkillValue(string skillName)
        {
            return _skillManager.GetSkillValue(skillName);
        }

        public int GetSkillIndex(string skillName)
        {
            return _skillManager.GetSkillIndex(skillName);
        }
        
        public float GetSkillRange(string skillName)
        {
            return _skillManager.GetSkillRange(skillName);
        }

        public void AttackEnemy(int value, float range)
        {
            var targetLayer = new LayerMask() {value = 1 << LayerMask.NameToLayer("Monster") };
            if (_battleSystem.CheckCollider(targetLayer, Vector2.right, range, out var targets))
            {
                foreach (var target in targets)
                {
                    _battleSystem.Attack(target);
                }
            }
            // TODO : value 주면 몬스터 때찌해주세요 (채이환)
        }

        public void HealMyself(float value)
        {
            // TODO : value 주면 체력 회복해주세요 (채이환)
        }

        public void AnimatorSetInteger(AnimationParameterEnums parameter, int value, Action action)
        {
            _animatorEventReceiver.SetInteger(_animationParameter[parameter], value, action);
        }

        public void AnimatorSetFloat(AnimationParameterEnums parameter, float value, Action action)
        {
            _animatorEventReceiver.SetFloat(_animationParameter[parameter], value, action);
        }

        public void AnimatorSetBool(AnimationParameterEnums parameter, bool value, Action action)
        {
            _animatorEventReceiver.SetBool(_animationParameter[parameter], value, action);
        }

        public int TakeDamage(int atk)
        {
            _healthSystem.Damage(atk);
            return atk;
        }

        public AnimatorStateInfo GetCurrentAnimatorStateInfo()
        {
            return _animatorEventReceiver.GetCurrentAnimatorStateInfo(0);
        }

        public AnimatorStateInfo GetNextAnimatorStateInfo()
        {
            return _animatorEventReceiver.GetNextAnimatorStateInfo(0);
        }

        public void RegistEvent(ECharacterEventType type, Action subscriber)
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

        public StateType GetCurrentStateType()
        {
            return _fsm.GetCurrentStateType();
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