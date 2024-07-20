using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;

namespace ScriptableObjects.Scripts.Creature.DTO
{

    public class SkillState : BaseState
    {
        //protected Animator _animator;
        //protected BattleSystem _battleSystem;

        //SkillStateInfo _skillInfo;

        //public SkillState(BaseStateInfo baseInfo, SkillStateInfo skillInfo, Func<StateType, bool> tryChangeState, BattleSystem battleSystem, Animator animator)
        public SkillState(BaseStateInfo baseInfo, Func<StateType, bool> tryChangeState)
            : base(baseInfo, tryChangeState)
        {
            //_skillInfo = skillInfo;
            //_battleSystem = battleSystem;
            //_animator = animator;
        }

        public override void Enter()
        {
            base.Enter();
            //_animator.SetBool(_baseStateInfo.stateParameter, true);
            //_animator.SetFloat(_skillInfo.skillParameter, _skillInfo.skillValue);
            //OnFixedUpdate += CheckTimeAndAttack;
        }
        public override void Exit()
        {
            base.Exit();
            //_animator.SetBool(_baseStateInfo.stateParameter, false);
            base.ClearEvent(Unit.GameScene.Stages.Creatures.Interfaces.EStateEventType.Enter);
            base.ClearEvent(Unit.GameScene.Stages.Creatures.Interfaces.EStateEventType.Exit);
            base.ClearEvent(Unit.GameScene.Stages.Creatures.Interfaces.EStateEventType.Update);
            base.ClearEvent(Unit.GameScene.Stages.Creatures.Interfaces.EStateEventType.FixedUpdate);
        }
        public override void Update()
        {
            base.Update();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        //protected virtual void CheckTimeAndAttack()
        //{
        //    OnFixedUpdate -= CheckTimeAndAttack;
        //    float duration = Time.time - _enterTime;
        //    if (duration > _skillInfo.actTiming)
        //    {
        //        _skillInfo.skillAct.Act();
        //    }
        //}
    }
}