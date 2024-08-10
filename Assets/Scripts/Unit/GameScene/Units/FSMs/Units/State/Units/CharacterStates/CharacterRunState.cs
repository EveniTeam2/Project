using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;
using Unit.GameScene.Units.FSMs.Units.State.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class CharacterRunState : CharacterBaseState
    {
        public CharacterRunState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
        {
        }

        public override void Enter()
        {
            CharacterMovementSystem.SetRun(true);
            SetBool(AnimationParameterEnums.Run, true, null);
        }

        public override void Update()
        {
            if (CharacterBattleSystem.CheckEnemyInRange(DefaultAttackRange, out RaycastHit2D[] _))
            {
                ChangeState(StateType.Idle);
            }
        }

        public override void Exit()
        {
            CharacterMovementSystem.SetRun(false);
            SetBool(AnimationParameterEnums.Run, false, null);
        }
    }
}