using System;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Units.StataMachine.Units;
using Unit.GameScene.Units.FSMs.Units.State.Abstract;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class MonsterSkillState : MonsterBaseState
    {
        private bool _isReadyForAttack;
        
        public MonsterSkillState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
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