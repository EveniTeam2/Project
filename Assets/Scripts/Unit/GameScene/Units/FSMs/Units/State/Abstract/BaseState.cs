using System;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Interfaces;
using Unit.GameScene.Units.FSMs.Units;
using UnityEngine;

public abstract class BaseState : IState
{
    protected float DefaultAttackRange;
    protected Vector3 _moveDirection;
    protected readonly StatSystem _statSystem;
    protected readonly MovementSystem _movementSystem;
    protected readonly BattleSystem _battleSystem;
    protected readonly SkillSystem _skillSystem;
    
    private readonly StateMachine _stateMachine;
    private readonly AnimationEventReceiver _animationEventReceiver;

    public BaseState(StateMachine stateMachine)
    {
        DefaultAttackRange = stateMachine.DefaultAttackRange;
        
        _statSystem = stateMachine.StatSystem;
        _movementSystem = stateMachine.MovementSystem;
        _battleSystem = stateMachine.BattleSystem;

        _stateMachine = stateMachine;
        _animationEventReceiver = stateMachine.AnimationEventReceiver;
        
        _moveDirection = stateMachine.CreatureType == CreatureType.Character ? Vector3.right : Vector3.left;
    }

    public virtual void Enter(){}
    public virtual void Update(){}
    public virtual void Exit(){}

    protected void SetBool(AnimationParameterEnums type, bool value, Action action)
    {
        _animationEventReceiver.SetBool(type, value, action);
    }

    protected void SetTrigger(AnimationParameterEnums type, Action action)
    {
        _animationEventReceiver.SetTrigger(type, action);
    }

    protected abstract void ChangeState(StateType stateType);
    
    protected void RegisterOnAttackEventHandler(Action onAttack)
    {
        _animationEventReceiver.OnActivateSkillEffect += onAttack;
    }

    protected void UnregisterOnAttackEventHandler(Action onAttack)
    {
        _animationEventReceiver.OnActivateSkillEffect -= onAttack;
    }
}