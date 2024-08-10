using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;
using Unit.GameScene.Units.FSMs.Units.State.Abstract;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class CharacterHitState : CharacterBaseState
    {
        public CharacterHitState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
        {
        }

        public override void Enter()
        {
            SetTrigger(AnimationParameterEnums.Hit, () => ChangeState(StateType.Idle));
        }
    }
}