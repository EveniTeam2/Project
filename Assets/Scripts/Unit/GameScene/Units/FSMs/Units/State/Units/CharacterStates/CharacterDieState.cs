using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;
using Unit.GameScene.Units.FSMs.Units.State.Abstract;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class CharacterDieState : CharacterBaseState
    {
        public CharacterDieState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
        {
            
        }

        public override void Enter()
        {
            SetTrigger(AnimationParameterEnums.Die, null);
        }
    }
}