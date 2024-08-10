using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.FSMs.Interfaces;
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
            if (!MonsterBattleSystem.CheckEnemyInRange(DefaultAttackRange, out RaycastHit2D[] _))
            {
                ChangeState(StateType.Run);
            }
            else if (MonsterBattleSystem.IsReadyForAttack)
            {
                ChangeState(StateType.Skill);
            }
        }

        public override void Exit()
        {
            SetBool(AnimationParameterEnums.Idle, false, null);
        }
    }
}