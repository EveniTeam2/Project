using System;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillControllers
{
    public interface IMonsterFsmController : IFsmController
    {
        public void RegisterOnAttackEventHandler(Action onAttack);
        public void UnregisterOnAttackEventHandler(Action onAttack);
        public bool IsReadyForAttack();
        public void AttackEnemy(RaycastHit2D target);
    }
}