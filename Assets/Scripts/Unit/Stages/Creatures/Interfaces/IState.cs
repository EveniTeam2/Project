using Unit.Stages.Creatures.FSM;

namespace Unit.Stages.Creatures.Interfaces
{
    public interface IState
    {
        void Enter(BaseCreature target);
        void Exit(BaseCreature target);
        void Update(BaseCreature target);
        void FixedUpdate(BaseCreature target);
        bool CanTransitionToThis(BaseCreature target);
        string StateName { get; }
        int ParameterHash { get; }
        StateMachine StateMachine { get; }
    }
}