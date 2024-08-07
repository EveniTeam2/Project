using System;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillControllers
{
    public interface ICharacterFsmController : IFsmController
    {
        void RegisterHandleOnCommandDequeue(Action action);
        void RegisterHandleOnPlayerDeath(Action action);
    }
}