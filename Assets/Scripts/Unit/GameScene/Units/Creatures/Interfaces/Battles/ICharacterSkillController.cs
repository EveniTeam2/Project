using System;
using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Creatures.Interfaces.Battles
{
    public interface ICharacterSkillController : ISkillController
    {
        public void SetReadyForInvokingCommand(bool isReady);
        public void TryChangeState(StateType stateType);
        public void SetBoolOnAnimator(AnimationParameterEnums animationParameterEnums, bool value, Action action);
        public void SetFloatOnAnimator(AnimationParameterEnums animationParameterEnums, float value, Action action);
    }
}