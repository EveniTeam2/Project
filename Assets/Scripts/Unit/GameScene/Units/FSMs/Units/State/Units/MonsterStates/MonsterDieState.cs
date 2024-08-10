using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.FSMs.Interfaces;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;
using Unit.GameScene.Units.FSMs.Units.State.Abstract;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class MonsterDieState : MonsterBaseState
    {
        public MonsterDieState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
        {
        }

        public override void Enter()
        {
            SetTrigger(AnimationParameterEnums.Die, null);
        }
    }
}