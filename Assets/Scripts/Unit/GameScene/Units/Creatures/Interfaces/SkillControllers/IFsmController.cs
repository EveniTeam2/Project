using System;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces.SkillControllers
{
    public interface IFsmController
    {
        bool CheckEnemyInRange(float range, out RaycastHit2D[] enemies);
        void ToggleMovement(bool setRunning);
        void SetBool(int parameter, bool value, Action action);
        void SetTrigger(int parameter, Action action);
        void SetFloat(int parameter, int value, Action action);
    }
}