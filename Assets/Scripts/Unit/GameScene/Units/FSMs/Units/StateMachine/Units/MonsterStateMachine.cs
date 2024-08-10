using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems;
using Unit.GameScene.Units.FSMs.Interfaces;

namespace Unit.GameScene.Units.FSMs.Units.StataMachine.Units
{
    public class MonsterStateMachine : StateMachine
    {
        public IState MonsterIdleState { get; private set; }
        public IState MonsterRunState { get; private set; }
        public IState MonsterHitState { get; private set; }
        public IState MonsterDieState { get; private set; }
        public IState MonsterSkillState { get; private set; }
        
        public MonsterStateMachine(float defaultAttackRange, AnimationEventReceiver animationEventReceiver, MonsterStatSystem monsterStatSystem, MonsterMovementSystem monsterMovementSystem, MonsterBattleSystem monsterBattleSystem, MonsterSkillSystem monsterSkillSystem)
            : base(defaultAttackRange, animationEventReceiver, monsterStatSystem, monsterMovementSystem, monsterBattleSystem, monsterSkillSystem)
        {
            MonsterIdleState = new MonsterIdleState(this);
            MonsterRunState = new MonsterRunState(this);
            MonsterHitState = new MonsterHitState(this);
            MonsterDieState = new MonsterDieState(this);
            MonsterSkillState = new MonsterSkillState(this);
            
            StateInfos.Add(StateType.Idle, MonsterIdleState);
            StateInfos.Add(StateType.Run, MonsterRunState);
            StateInfos.Add(StateType.Hit, MonsterHitState);
            StateInfos.Add(StateType.Die, MonsterDieState);
            StateInfos.Add(StateType.Skill, MonsterSkillState);
        }
    }
}