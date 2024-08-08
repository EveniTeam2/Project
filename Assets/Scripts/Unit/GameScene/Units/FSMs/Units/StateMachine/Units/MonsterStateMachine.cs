using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;

namespace Unit.GameScene.Units.FSMs.Units.StataMachine.Units
{
    public class MonsterStateMachine : StateMachine
    {
        public MonsterStateMachine(CreatureType creatureType, float defaultAttackRange, AnimationEventReceiver animationEventReceiver, StatSystem statSystem, MovementSystem movementSystem, BattleSystem battleSystem) : base(creatureType, defaultAttackRange, animationEventReceiver, statSystem, movementSystem, battleSystem)
        {
        }
    }
}