using System;
using System.Collections.Generic;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules
{
    public class MonsterServiceProvider : ICreatureServiceProvider
    {
        private readonly AnimatorEventReceiver _animatorEventReceiver;
        private readonly BattleSystem _battleSystem;
        private readonly StateMachine _fsm;
        private readonly HealthSystem _healthSystem;
        private readonly MovementSystem _movementSystem;
        private readonly Dictionary<AnimationParameterEnums, int> _animationParameter;
        private readonly Dictionary<string, int> _skillIndexes;

        public MonsterServiceProvider(
            BattleSystem battleSystem,
            HealthSystem healthSystem,
            MovementSystem movementSystem,
            AnimatorEventReceiver animatorEventReceiver,
            StateMachine fsm,
            Dictionary<AnimationParameterEnums, int> animationParameter,
            Dictionary<string, int> skillIndexes)
        {
            _battleSystem = battleSystem;
            _healthSystem = healthSystem;
            _movementSystem = movementSystem;
            _animatorEventReceiver = animatorEventReceiver;
            _fsm = fsm;
            _animationParameter = animationParameter;
            _skillIndexes = skillIndexes;
        }

        public int GetSkillIndex(string skillName)
        {
            return _skillIndexes[skillName];
        }

        public void AnimatorSetInteger(AnimationParameterEnums parameter, int value, Action action)
        {
            _animatorEventReceiver.SetInteger(_animationParameter[parameter], value, null);
        }

        public void AnimatorSetFloat(AnimationParameterEnums parameter, float value, Action action)
        {
            _animatorEventReceiver.SetFloat(_animationParameter[parameter], value, null);
        }

        public void AnimatorSetBool(AnimationParameterEnums parameter, bool value, Action action)
        {
            _animatorEventReceiver.SetBool(_animationParameter[parameter], value, null);
        }

        public bool TryChangeState(StateType stateType)
        {
            _fsm.TryChangeState(stateType);
            return false;
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
            _fsm.TryChangeState(StateType.Run);
            _movementSystem.SetRun(isRun);
        }

        public void RegistEventSkill(Action OnEnter, Action OnExit, Action OnUpdate, Action OnFixedUpdate)
        {
            _fsm.RegistOnSkillState(OnEnter, OnExit, OnUpdate, OnFixedUpdate);
        }

        //public void Attack(RaycastHit2D hit)
        //{
        //    _battleSystem.Attack(hit);
        //}

        //public bool CheckCollider(LayerMask targetLayer, Vector2 direction, float _distance,
        //    out RaycastHit2D[] collider)
        //{
        //    return _battleSystem.CheckCollider(targetLayer, direction, _distance, out collider);
        //}

        //public void RegistStateEvent(StateType stateType, EStateEventType eventType, Action subscriber)
        //{
        //    if (_fsm.TryGetState(stateType, out var state))
        //        switch (eventType)
        //        {
        //            case EStateEventType.Enter:
        //                state.OnEnter += subscriber;
        //                break;
        //            case EStateEventType.Exit:
        //                state.OnExit += subscriber;
        //                break;
        //        }
        //}

        //public void UnregistStateEvent(StateType stateType, EStateEventType eventType, Action subscriber)
        //{
        //    if (_fsm.TryGetState(stateType, out var state))
        //        switch (eventType)
        //        {
        //            case EStateEventType.Enter:
        //                state.OnEnter -= subscriber;
        //                break;
        //            case EStateEventType.Exit:
        //                state.OnExit -= subscriber;
        //                break;
        //        }
        //}
    }
}