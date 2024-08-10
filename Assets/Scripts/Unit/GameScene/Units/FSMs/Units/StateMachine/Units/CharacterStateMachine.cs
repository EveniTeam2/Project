using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems;
using Unit.GameScene.Units.FSMs.Interfaces;

namespace Unit.GameScene.Units.FSMs.Units.StataMachine.Units
{
    public class CharacterStateMachine : StateMachine
    {
        public IState CharacterIdleState { get; private set; }
        public IState CharacterRunState { get; private set; }
        public IState CharacterHitState { get; private set; }
        public IState CharacterDieState { get; private set; }
        public IState CharacterSkillState { get; private set; }
        
        public CharacterStateMachine(float defaultAttackRange, AnimationEventReceiver animationEventReceiver, CharacterStatSystem characterStatSystem, CharacterMovementSystem characterMovementSystem, CharacterBattleSystem characterBattleSystem, CharacterSkillSystem characterSkillSystem)
            : base(defaultAttackRange, animationEventReceiver, characterStatSystem, characterMovementSystem, characterBattleSystem, characterSkillSystem)
        {
            CharacterIdleState = new CharacterIdleState(this);
            CharacterRunState = new CharacterRunState(this);
            CharacterHitState = new CharacterHitState(this);
            CharacterDieState = new CharacterDieState(this);
            CharacterSkillState = new CharacterSkillState(this);
            
            StateInfos.Add(StateType.Idle, CharacterIdleState);
            StateInfos.Add(StateType.Run, CharacterRunState);
            StateInfos.Add(StateType.Hit, CharacterHitState);
            StateInfos.Add(StateType.Die, CharacterDieState);
            StateInfos.Add(StateType.Skill, CharacterSkillState);
        }
    }
}