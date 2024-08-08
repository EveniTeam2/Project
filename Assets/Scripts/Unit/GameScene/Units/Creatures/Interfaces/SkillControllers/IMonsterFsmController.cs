using System;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillControllers
{
    public interface IMonsterFsmController : IFsmController
    {
        void RegisterOnAttackEventHandler(Action onAttack);
        void UnregisterOnAttackEventHandler(Action onAttack);
        bool IsReadyForAttack();
        void AttackEnemy(RaycastHit2D target);
        bool CheckPlayer(Vector2 startPos, Vector2 endPos, out RaycastHit2D[] target);
        bool CheckPlayer(Vector2 direction, float distance, out RaycastHit2D[] target);
    }
}