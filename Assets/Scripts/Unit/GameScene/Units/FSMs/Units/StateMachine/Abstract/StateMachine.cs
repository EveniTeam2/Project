using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems;
using Unit.GameScene.Units.FSMs.Interfaces;

namespace Unit.GameScene.Units.FSMs.Units
{
    public class StateMachine
    {
        public float DefaultAttackRange { get; private set; }
        public AnimationEventReceiver AnimationEventReceiver { get; private set; }
        public StatSystem StatSystem { get; private set; }
        public MovementSystem MovementSystem { get; private set; }
        public BattleSystem BattleSystem { get; private set; }
        public SkillSystem SkillSystem { get; private set; }

        protected readonly Dictionary<StateType, IState> StateInfos = new ();
        
        private IState _currentState;

        protected StateMachine(float defaultAttackRange, AnimationEventReceiver animationEventReceiver,
            StatSystem statSystem, MovementSystem movementSystem, BattleSystem battleSystem,
            SkillSystem skillSystem)
        {
            DefaultAttackRange = defaultAttackRange;
            AnimationEventReceiver = animationEventReceiver;
            StatSystem = statSystem;
            MovementSystem = movementSystem;
            BattleSystem = battleSystem;
            SkillSystem = skillSystem;
        }

        public void ChangeState(IState newState)
        {
            _currentState?.Exit();

            _currentState = newState;

            _currentState?.Enter();
        }

        public void ChangeState(StateType stateType)
        {
            ChangeState(StateInfos[stateType]);
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public StateType GetCurrentStateType()
        {
            return (from stateInfo in StateInfos where stateInfo.Value == _currentState select stateInfo.Key).FirstOrDefault();
        }
    }
}