using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.FSMs.Interfaces;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;
using Unit.GameScene.Units.FSMs.Units.State.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class MonsterRunState : MonsterBaseState
    {
        public MonsterRunState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
        {
        }

        public override void Enter()
        {
            MonsterMovementSystem.SetRun(true);
            SetBool(AnimationParameterEnums.Run, true, null);
        }

        public override void Update()
        {
            if (MonsterBattleSystem.CheckEnemyInRange(DefaultAttackRange, out RaycastHit2D[] _))
            {
                ChangeState(StateType.Idle);
            }
        }

        public override void Exit()
        {
            MonsterMovementSystem.SetRun(false);
            SetBool(AnimationParameterEnums.Run, false, null);
        }
    }
}