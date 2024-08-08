using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Interfaces;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class StateMachine
    {
        public CreatureType CreatureType { get; private set; }
        public float DefaultAttackRange { get; private set; }
        public AnimationEventReceiver AnimationEventReceiver { get; private set; }
        public StatSystem StatSystem { get; private set; }
        public MovementSystem MovementSystem { get; private set; }
        public BattleSystem BattleSystem { get; private set; }

        protected Dictionary<StateType, IState> StateInfos = new ();
        
        private IState _currentState;

        protected StateMachine(CreatureType creatureType, float defaultAttackRange, AnimationEventReceiver animationEventReceiver, StatSystem statSystem, MovementSystem movementSystem, BattleSystem battleSystem)
        {
            CreatureType = creatureType;
            DefaultAttackRange = defaultAttackRange;
            AnimationEventReceiver = animationEventReceiver;
            StatSystem = statSystem;
            MovementSystem = movementSystem;
            BattleSystem = battleSystem;
        }

        public void ChangeState(IState newState)
        {
            _currentState?.Exit();

            _currentState = newState;

            _currentState?.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
        }
    }
}