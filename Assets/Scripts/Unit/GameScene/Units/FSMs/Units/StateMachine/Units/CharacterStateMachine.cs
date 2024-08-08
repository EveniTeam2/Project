using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
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
        
        public CharacterStateMachine(CreatureType creatureType, float defaultAttackRange, AnimationEventReceiver animationEventReceiver, StatSystem statSystem, MovementSystem movementSystem, BattleSystem battleSystem) : base(creatureType, defaultAttackRange, animationEventReceiver, statSystem, movementSystem, battleSystem)
        {
            StateInfos.AD
            CharacterIdleState = new CharacterIdleState(this);
            CharacterRunState = new CharacterRunState(this);
            CharacterHitState = new CharacterHitState(this);
            CharacterDieState = new CharacterDieState(this);
            CharacterSkillState = new CharacterSkillState(this);
        }
    }
}