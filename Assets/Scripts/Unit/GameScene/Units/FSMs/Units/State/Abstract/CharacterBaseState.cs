using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;

namespace Unit.GameScene.Units.FSMs.Units.State.Abstract
{
    public class CharacterBaseState : BaseState
    {
        private readonly CharacterStateMachine _characterStateMachine;
        
        public CharacterBaseState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
        {
            _characterStateMachine = characterStateMachine;
        }

        protected override void ChangeState(StateType stateType)
        {
            switch (stateType)
            {
                case StateType.Idle:
                    _characterStateMachine.ChangeState(_characterStateMachine.CharacterIdleState);
                    break;
                case StateType.Run:
                    _characterStateMachine.ChangeState(_characterStateMachine.CharacterRunState);
                    break;
                case StateType.Skill:
                    _characterStateMachine.ChangeState(_characterStateMachine.CharacterSkillState);
                    break;
                case StateType.Die:
                    _characterStateMachine.ChangeState(_characterStateMachine.CharacterDieState);
                    break;
                case StateType.Hit:
                    _characterStateMachine.ChangeState(_characterStateMachine.CharacterHitState);
                    break;
            }
        }
    }
}