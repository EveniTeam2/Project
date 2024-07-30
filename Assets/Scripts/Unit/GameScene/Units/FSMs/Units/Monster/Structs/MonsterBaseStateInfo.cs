using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.FSMs.Units.Monster.Structs
{
    public struct MonsterBaseStateInfo
    {
        public readonly StateType StateType;
        public readonly int StateParameter;
        public readonly StateType DefaultExitState;

        public MonsterBaseStateInfo(StateType stateType, int stateParameter, StateType defaultExitState)
        {
            StateType = stateType;
            StateParameter = stateParameter;
            DefaultExitState = defaultExitState;
        }
    }
}