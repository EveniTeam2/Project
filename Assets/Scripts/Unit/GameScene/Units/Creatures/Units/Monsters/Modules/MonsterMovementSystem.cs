using System;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules
{
    public class MonsterMovementSystem : MovementSystem
    {
        public MonsterMovementSystem(Transform targetTransform, MonsterMovementStat stats, float ground) : base(
            targetTransform, stats, ground)
        {
        }

        public override void SetRun(bool isRun)
        {
#if UNITY_EDITOR
            //Debug.Log($"Run State:{isRun}");
#endif
            _wantToMove = isRun;
            if (isRun)
                _targetSpd = -1 * _stats.GetSpeed();
            else
                _targetSpd = 0;
        }

        public override void SetBackward(bool isBack)
        {
            _wantToMove = isBack;
            if (_wantToMove)
                _targetSpd = _stats.GetSpeed();
            else
                _targetSpd = 0;
        }

        public override void Jump(float power)
        {
        }

        public override void SpawnInit(IMovementStat movementStat)
        {
            _stats = movementStat;
        }
    }

    public class MonsterMovementStat : IMovementStat
    {
        protected Func<int> _getSpeed;

        public MonsterMovementStat(Stat<MonsterStat> stats)
        {
            _getSpeed = () => stats.Current.Speed;
        }

        public int GetSpeed()
        {
            return _getSpeed();
        }
    }
}