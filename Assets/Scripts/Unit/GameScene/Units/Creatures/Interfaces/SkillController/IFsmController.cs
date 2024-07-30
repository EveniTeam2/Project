using System;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillController
{
    public interface IFsmController
    {
        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float range, out RaycastHit2D[] enemies);
        public void ToggleMovement(bool setRunning);
        
        void SetBool(int parameter, bool value, Action action);
        void SetTrigger(int parameter, Action action);
        void SetInteger(int parameter, int value, Action action);
    }
}