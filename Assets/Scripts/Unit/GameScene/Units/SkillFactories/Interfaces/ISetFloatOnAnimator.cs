using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.SkillFactories.Interfaces
{
    public interface ISetFloatOnAnimator
    {
        public void SetFloatOnAnimator(AnimationParameterEnums targetParameter, int value);
    }
}