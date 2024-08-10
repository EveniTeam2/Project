using System;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Interfaces;
using Unit.GameScene.Units.FSMs.Units;
using UnityEngine;

public abstract class BaseState : IState
{
    protected readonly float DefaultAttackRange;
    
    private readonly StateMachine _stateMachine;
    private readonly AnimationEventReceiver _animationEventReceiver;

    protected BaseState(StateMachine stateMachine)
    {
        DefaultAttackRange = stateMachine.DefaultAttackRange;

        _stateMachine = stateMachine;
        _animationEventReceiver = stateMachine.AnimationEventReceiver;
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