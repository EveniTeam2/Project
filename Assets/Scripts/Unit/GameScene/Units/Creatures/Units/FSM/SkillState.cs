using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.Creatures.Units.FSM
{

    public class SkillState : BaseState
    {
        public SkillState(BaseStateInfo baseInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver)
            : base(baseInfo, tryChangeState, animatorEventReceiver)
        {

        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Exit()
        {
            base.Exit();
            base.ClearEvent(EStateEventType.Enter);
            base.ClearEvent(EStateEventType.Exit);
            base.ClearEvent(EStateEventType.Update);
            base.ClearEvent(EStateEventType.FixedUpdate);
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