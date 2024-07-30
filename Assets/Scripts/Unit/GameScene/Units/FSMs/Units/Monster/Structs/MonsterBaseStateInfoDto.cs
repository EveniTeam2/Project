using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.FSMs.Units.Monster.Structs
{
    [Serializable]
    public struct MonsterBaseStateInfoDto
    {
        public StateType stateType;
        public AnimationParameterEnums stateParameter;
        public StateType defaultExitState;

        public MonsterBaseStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new MonsterBaseStateInfo(stateType, animationParameterEnums[stateParameter], defaultExitState);
        }
    }
}