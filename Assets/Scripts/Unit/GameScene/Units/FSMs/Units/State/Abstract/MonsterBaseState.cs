using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;

namespace Unit.GameScene.Units.FSMs.Units.State.Abstract
{
    public class MonsterBaseState : BaseState
    {
        private readonly MonsterStateMachine _monsterStateMachine;
        protected MonsterBaseState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
        {
            _monsterStateMachine = monsterStateMachine;
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        protected override void ChangeState(StateType stateType)
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}