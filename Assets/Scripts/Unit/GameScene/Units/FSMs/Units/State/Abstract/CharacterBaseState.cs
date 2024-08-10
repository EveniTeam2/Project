using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;

namespace Unit.GameScene.Units.FSMs.Units.State.Abstract
{
    public class CharacterBaseState : BaseState
    {
        private readonly CharacterStateMachine _characterStateMachine;
        
        protected readonly CharacterStatSystem CharacterStatSystem;
        protected readonly CharacterMovementSystem CharacterMovementSystem;
        protected readonly CharacterBattleSystem CharacterBattleSystem;
        protected readonly CharacterSkillSystem CharacterSkillSystem;
        
        public CharacterBaseState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
        {
            _characterStateMachine = characterStateMachine;
            CharacterStatSystem = _characterStateMachine.StatSystem as CharacterStatSystem;
            CharacterMovementSystem = _characterStateMachine.MovementSystem as CharacterMovementSystem;
            CharacterBattleSystem = _characterStateMachine.BattleSystem as CharacterBattleSystem;
            CharacterSkillSystem = _characterStateMachine.SkillSystem as CharacterSkillSystem;
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