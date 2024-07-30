using System;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillController
{
    public interface ISetValueOnAnimator
    {
        void SetBoolOnAnimator(AnimationParameterEnums targetParameter, bool value, Action action);
        void SetFloatOnAnimator(AnimationParameterEnums targetParameter, int value, Action action);
    }
}