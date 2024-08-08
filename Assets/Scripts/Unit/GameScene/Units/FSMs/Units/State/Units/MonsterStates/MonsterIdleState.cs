using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;
using Unit.GameScene.Units.FSMs.Units.State.Abstract;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class MonsterIdleState : MonsterBaseState
    {
        public MonsterIdleState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
        {
        }

        public override void Enter()
        {
            SetBool(AnimationParameterEnums.Idle, true, null);
        }

        public override void Update()
        {
            ChangeState(_battleSystem.CheckEnemyInRange(DefaultAttackRange, out RaycastHit2D[] _) ? StateType.Skill : StateType.Run);
        }

        public override void Exit()
        {
            SetBool(AnimationParameterEnums.Idle, false, null);
        }
    }
}