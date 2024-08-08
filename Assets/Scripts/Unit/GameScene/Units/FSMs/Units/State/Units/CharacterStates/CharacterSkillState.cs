using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;
using Unit.GameScene.Units.FSMs.Units.State.Abstract;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class CharacterSkillState : CharacterBaseState
    {
        private bool _isReadyForAttack;
        
        public CharacterSkillState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
        {
        }

        public override void Enter()
        {
            _isReadyForAttack = false;
            SetBool(AnimationParameterEnums.Skill, true, () => ChangeState(StateType.Idle));
        }

        public override void Update() { }

        public override void Exit()
        {
            SetBool(AnimationParameterEnums.Skill, false, null);
        }
    }
}