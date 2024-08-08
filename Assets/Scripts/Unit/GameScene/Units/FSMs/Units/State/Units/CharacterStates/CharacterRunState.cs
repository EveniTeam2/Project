using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;
using Unit.GameScene.Units.FSMs.Units.State.Abstract;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class CharacterRunState : CharacterBaseState
    {
        public CharacterRunState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
        {
        }

        public override void Enter()
        {
            _movementSystem.SetRun(true);
            SetBool(AnimationParameterEnums.Run, true, null);
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            _movementSystem.SetRun(false);
            SetBool(AnimationParameterEnums.Run, false, null);
        }
    }
}