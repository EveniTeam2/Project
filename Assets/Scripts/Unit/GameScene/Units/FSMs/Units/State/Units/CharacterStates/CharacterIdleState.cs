using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;
using Unit.GameScene.Units.FSMs.Units.State.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class CharacterIdleState : CharacterBaseState
    {
        public CharacterIdleState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
        {
        }

        public override void Enter()
        {
            SetBool(AnimationParameterEnums.Idle, true, null);
        }

        public override void Update()
        {
            if (!CharacterBattleSystem.CheckEnemyInRange(DefaultAttackRange, out RaycastHit2D[] _))
            {
                ChangeState(StateType.Run);
            }
        }

        public override void Exit()
        {
            SetBool(AnimationParameterEnums.Idle, false, null);
        }
    }
}