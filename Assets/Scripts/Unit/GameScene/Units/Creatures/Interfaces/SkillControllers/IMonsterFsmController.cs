using System;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules;
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