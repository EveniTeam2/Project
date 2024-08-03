using System;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillControllers
{
    public interface IFsmController
    {
        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float range, out RaycastHit2D[] enemies);
        public void ToggleMovement(bool setRunning);
        
        void SetBool(int parameter, bool value, Action action);
        void SetTrigger(int parameter, Action action);
        void SetFloat(int parameter, int value, Action action);
    }
}