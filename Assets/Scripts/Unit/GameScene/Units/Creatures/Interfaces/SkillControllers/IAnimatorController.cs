using System;
using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillControllers
{
    public interface IAnimatorController
    {
        void SetBoolOnAnimator(AnimationParameterEnums targetParameter, bool value, Action action);
        void SetFloatOnAnimator(AnimationParameterEnums targetParameter, int value, Action action);
    }
}