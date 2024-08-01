using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces.Movements
{
    public interface ICreatureMovement
    {
        public void SetRun(bool isRun);
        public void SetBackward(bool isBackward);
        public void SetJump(float power);
        public void SetGroundPosition(float groundYPosition);
        public void SetImpact(float duration);
    }
}