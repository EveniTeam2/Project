using System;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillController
{
    public interface IMonsterFsmController : IFsmController
    {
        public bool IsReadyForAttack();
        public IBattleStat GetBattleStat();
        public event Action OnAttack;
        public void Attack(RaycastHit2D target);
    }
}