using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;

namespace ScriptableObjects.Scripts.Creature.DTO
{

    public class SkillState : BaseState
    {
        public SkillState(BaseStateInfo baseInfo, Func<StateType, bool> tryChangeState)
            : base(baseInfo, tryChangeState)
        {

        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Exit()
        {
            base.Exit();
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
    }
}