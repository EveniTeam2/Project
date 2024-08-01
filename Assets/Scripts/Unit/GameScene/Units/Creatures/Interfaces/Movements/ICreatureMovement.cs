using System.Numerics;

namespace Unit.GameScene.Units.Creatures.Interfaces.Movements
{
    public interface ICreatureMovement
    {
        public void SetRun(bool isRun);
        public void SetBackward(bool isBackward);
        public void Jump(float power);
        public void SetGroundPosition(float groundYPosition);
        public void SetImpact(Vector2 impact, float duration);
    }
}