using System;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillControllers
{
    public interface IMonsterFsmController : IFsmController
    {
        void RegisterOnAttackEventHandler(Action onAttack);
        void UnregisterOnAttackEventHandler(Action onAttack);
        bool IsReadyForAttack();
    }
}