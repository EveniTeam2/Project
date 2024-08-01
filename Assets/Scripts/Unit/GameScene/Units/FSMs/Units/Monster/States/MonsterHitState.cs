using System;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.FSMs.Units.Monster.Structs;

namespace Unit.GameScene.Units.FSMs.Units.Monster.States
{
    public class MonsterHitState : MonsterBaseState
    {
        public MonsterHitState(MonsterBaseStateInfo monsterBaseStateInfo, Func<StateType, bool> tryChangeState, IMonsterFsmController fsmController)
            : base(monsterBaseStateInfo, tryChangeState, fsmController) { }

        public override void Enter()
        {
            base.Enter();
            FsmController.SetTrigger(MonsterBaseStateInfo.StateParameter, ChangeToDefaultState);
        }
    }
}