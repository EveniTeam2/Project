using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;

namespace Unit.GameScene.Units.FSMs.Units.State.Abstract
{
    public class MonsterBaseState : BaseState
    {
        private readonly MonsterStateMachine _monsterStateMachine;
        
        protected readonly MonsterStatSystem MonsterStatSystem;
        protected readonly MonsterMovementSystem MonsterMovementSystem;
        protected readonly MonsterBattleSystem MonsterBattleSystem;
        protected readonly MonsterSkillSystem MonsterSkillSystem;
        
        protected MonsterBaseState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
        {
            _monsterStateMachine = monsterStateMachine;
            MonsterStatSystem = _monsterStateMachine.StatSystem as MonsterStatSystem;
            MonsterMovementSystem = _monsterStateMachine.MovementSystem as MonsterMovementSystem;
            MonsterBattleSystem = _monsterStateMachine.BattleSystem as MonsterBattleSystem;
            MonsterSkillSystem = _monsterStateMachine.SkillSystem as MonsterSkillSystem;
        }
        
        protected override void ChangeState(StateType stateType)
        {
            switch (stateType)
            {
                case StateType.Idle:
                    _monsterStateMachine.ChangeState(_monsterStateMachine.MonsterIdleState);
                    break;
                case StateType.Run:
                    _monsterStateMachine.ChangeState(_monsterStateMachine.MonsterRunState);
                    break;
                case StateType.Skill:
                    _monsterStateMachine.ChangeState(_monsterStateMachine.MonsterSkillState);
                    break;
                case StateType.Die:
                    _monsterStateMachine.ChangeState(_monsterStateMachine.MonsterDieState);
                    break;
                case StateType.Hit:
                    _monsterStateMachine.ChangeState(_monsterStateMachine.MonsterHitState);
                    break;
            }
        }
    }
}